using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Hikari.Puzzle {
    public class Game {
        private readonly Match match;
        private readonly int id;
        public PlayerInfo Player { get; set; }
        public Board Board { get; }
        public Piece? CurrentPiece { get; private set; }
        public Piece? Ghost { get; private set; }
        public uint Damage { get; private set; }
        public bool IsHoldLocked { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsDead { get; private set; }

        public IController Controller { get; set; }

        public float Gravity { get; } = 2f;
        private float yTimer = 0f;
        private float autoLockTimer = 0f;
        private int spawnDelay = -1;
        private uint pendingAttack = 0;

        public IObservable<IGameEvent> EventStream { get; }
        private readonly Subject<IGameEvent> eventSubject;

        private readonly Stopwatch stopwatch = new Stopwatch();
        private int totalAttack;
        private int totalLines;
        private int piecesPlaced;

        public float APM => totalAttack * 60 / (stopwatch.ElapsedMilliseconds / 1000f);
        public float LPM => totalLines * 60 / (stopwatch.ElapsedMilliseconds / 1000f);
        public float PPS => piecesPlaced / (stopwatch.ElapsedMilliseconds / 1000f);

        public static float GarbageColumnRandomness { get; } = 0.3f;
        public static int LineClearDelay { get; } = 26;

        public Game(int id, Match match, PlayerInfo player = default) {
            eventSubject = eventSubject = new Subject<IGameEvent>();
            EventStream = eventSubject.AsObservable();
            this.match = match;
            this.id = id;
            Player = player;
            Board = new Board(5);
            CurrentPiece = null;

            this.match.OnCountDown += OnMatchCountDown;
            this.match.OnMatchStart += () => {
                    stopwatch.Start();
                    SpawnNewPiece();
                };
            this.match.OnUpdate += Update;
        }

        private void OnMatchCountDown(int c) {
            foreach (var piece in Board.nextPieces) {
                eventSubject.OnNext(new QueueUpdatedEvent(piece));
            }

            eventSubject.OnNext(new InitializedEvent());
            match.OnCountDown -= OnMatchCountDown;
        }

        private void SpawnNewPiece() {
            spawnDelay = -1;
            var kind = Board.Next();
            CheckSpawnPiece(kind);
            
            if (IsDead) return;

            Ghost = Board.SonicDrop(CurrentPiece.Value);
            yTimer = 0f;
            autoLockTimer = 0f;
            eventSubject.OnNext(new PieceSpawnedEvent(CurrentPiece.Value.Kind));
            eventSubject.OnNext(new QueueUpdatedEvent(Board.nextPieces.Last()));
            IsHoldLocked = false;
        }

        private void CheckSpawnPiece(PieceKind kind) {
            CurrentPiece = new Piece(kind, 3, kind == PieceKind.I ? 17 : 18, 0);
            if (Board.Collides(CurrentPiece.Value)) CurrentPiece = CurrentPiece.Value.WithOffset(Vector2Int.up);
            if (Board.Collides(CurrentPiece.Value)) {
                IsDead = true;
                Debug.Log($"Player{id} died");
                eventSubject.OnNext(DeathEvent.Default);
            }
        }

        public void Update() {
            if (spawnDelay == 0) {
                if (Damage > pendingAttack) {
                    Damage -= pendingAttack;
                    AddGarbageLines(Damage);
                } else if (pendingAttack != Damage) {
                    match.DistributeDamage(id, pendingAttack - Damage);
                }
                pendingAttack = 0;
                Damage = 0;
                
                SpawnNewPiece();
            }

            if (spawnDelay > 0) {
                spawnDelay--;
                return;
            }

            if (IsDead) return;

            if (!CurrentPiece.HasValue) return;
            var currentPiece = CurrentPiece.Value;

            var updated = false;
            var gravityMultiplier = 1f;

            if (Controller != null) {
                var cmd = Controller.RequestControlUpdate();

                if ((cmd & Command.Hold) != 0) {
                    if (IsHoldLocked) return;
                    if (Board.holdPiece.HasValue) {
                        var tmp = Board.holdPiece.Value;
                        Board.holdPiece = currentPiece.Kind;
                        CheckSpawnPiece(tmp);
                        Ghost = Board.SonicDrop(CurrentPiece.Value);
                    } else {
                        Board.holdPiece = currentPiece.Kind;
                        Board.initialBag.Take(currentPiece.Kind);
                        SpawnNewPiece();
                    }

                    IsHoldLocked = true;
                    eventSubject.OnNext(HoldEvent.Default);

                    return;
                }

                if ((cmd & Command.RotateLeft) != 0) {
                    if (SRS.TryRotate(currentPiece, Board, false, out var result)) {
                        var spinStatus = Board.CheckTSpin(result.Item2, result.Item1);
                        currentPiece = result.Item2.WithTSpinStatus(spinStatus);
                        updated = true;
                    }
                }

                if ((cmd & Command.RotateRight) != 0) {
                    if (SRS.TryRotate(currentPiece, Board, true, out var result)) {
                        var spinStatus = Board.CheckTSpin(result.Item2, result.Item1);
                        currentPiece = result.Item2.WithTSpinStatus(spinStatus);
                        updated = true;
                    }
                }

                if ((cmd & Command.Left) != 0) {
                    currentPiece = currentPiece.WithOffset(Vector2Int.left);
                    if (Board.Collides(currentPiece)) {
                        currentPiece = currentPiece.WithOffset(Vector2Int.right);
                    } else {
                        updated = true;
                    }
                }

                if ((cmd & Command.Right) != 0) {
                    currentPiece = currentPiece.WithOffset(Vector2Int.right);
                    if (Board.Collides(currentPiece)) {
                        currentPiece = currentPiece.WithOffset(Vector2Int.left);
                    } else {
                        updated = true;
                    }
                }

                if ((cmd & Command.SoftDrop) != 0) gravityMultiplier = 20f;
                if ((cmd & Command.HardDrop) != 0) {
                    CurrentPiece = currentPiece;
                    LockPiece();
                }
            }

            Ghost = Board.SonicDrop(currentPiece);
            yTimer += Gravity * Time.deltaTime * gravityMultiplier;
            var fall = false;
            while (yTimer > 1) {
                if (currentPiece == Ghost.Value) {
                    yTimer = 0;
                    if (fall) updated = true;
                    break;
                }

                fall = true;
                currentPiece = currentPiece.WithOffset(Vector2Int.down);
                yTimer--;
                updated = true;
            }

            IsGrounded = currentPiece == Ghost.Value;

            if (updated) {
                CurrentPiece = currentPiece;
                eventSubject.OnNext(FallingPieceMovedEvent.Default);
            }
        }

        private void LockPiece() {
            if (!CurrentPiece.HasValue) throw new NullReferenceException($"{nameof(CurrentPiece)} is null");
            var drop = Board.SonicDrop(CurrentPiece.Value);
            var prevB2B = Board.b2b;
            var lockResult = Board.Lock(drop);
            piecesPlaced++;
            eventSubject.OnNext(new PieceLockedEvent(true, drop, lockResult, prevB2B));
            CurrentPiece = null;
            Ghost = null;
            spawnDelay = 3;

            totalLines += lockResult.clearedLines.Count;

            if (lockResult.placementKind.IsLineClear() && !lockResult.perfectClear) spawnDelay += 18 + 20;

            var attack = lockResult.attack;
            totalAttack += (int) attack;

            if (lockResult.attack > 0) {
                pendingAttack = attack;
            }
        }

        public void AddDamage(uint attack) {
            Damage += attack;
            eventSubject.OnNext(new GotDamageEvent(attack));
        }

        private void AddGarbageLines(uint amount) {
            var columnPos = Random.Range(0, 10);
            var lines = new List<byte[]>();
            for (var i = 0; i < amount; i++) {
                if (Random.Range(0f, 1f) < GarbageColumnRandomness) {
                    columnPos = Random.Range(0, 10);
                }

                var line = new byte[10];
                for (var j = 0; j < line.Length; j++) {
                    line[j] = (byte) (columnPos == j ? 0 : 7);
                }

                lines.Add(line);
            }

            var array = lines.ToArray();
            Board.InsertRowsAtBottom(array);
            eventSubject.OnNext(new GarbageLinesAddedEvent(array));
        }

        public bool IsCurrentPieceGrounded {
            get {
                if (!CurrentPiece.HasValue || !Ghost.HasValue) return false;
                return CurrentPiece.Value == Ghost.Value;
            }
        }

        public static readonly int[] RenAttacks = {
            0, 0, // 0, 1 combo
            1, 1, // 2, 3 combo
            2, 2, // 4, 5 combo
            3, 3, // 6, 7 combo
            4, 4, 4, // 8, 9, 10 combo
            5 // 11+ combo
        };

        public static int GetRenAttack(int renCount) => RenAttacks[math.clamp(renCount, 0, 11)];
        public static int GetRenAttack(uint renCount) => RenAttacks[math.min(renCount, 11)];

        public struct PlayerInfo {
            public string Name { get; set; }
            public PlayerKind Kind { get; set; }
        }

        public interface IGameEvent { }

        public class QueueUpdatedEvent : IGameEvent {
            public PieceKind kind;

            public QueueUpdatedEvent(PieceKind kind) {
                this.kind = kind;
            }
        }

        public class InitializedEvent : IGameEvent { }

        public class PieceSpawnedEvent : IGameEvent {
            public PieceKind kind;

            public PieceSpawnedEvent(PieceKind kind) {
                this.kind = kind;
            }
        }

        public class FallingPieceMovedEvent : IGameEvent {
            public static FallingPieceMovedEvent Default { get; } = new FallingPieceMovedEvent();
        }

        public class PieceLockedEvent : IGameEvent {
            public bool hardDrop;
            public Piece piece;
            public LockResult lockResult;
            public bool prevB2B;

            public PieceLockedEvent(bool hardDrop, Piece piece, LockResult lockResult, bool prevB2B) {
                this.hardDrop = hardDrop;
                this.piece = piece;
                this.lockResult = lockResult;
                this.prevB2B = prevB2B;
            }
        }

        public class HoldEvent : IGameEvent {
            public static HoldEvent Default { get; } = new HoldEvent();
        }

        public class GotDamageEvent : IGameEvent {
            public uint value;

            public GotDamageEvent(uint value) {
                this.value = value;
            }
        }

        public class GarbageLinesAddedEvent : IGameEvent {
            public byte[][] rows;

            public GarbageLinesAddedEvent(byte[][] rows) {
                this.rows = rows;
            }
        }

        public class DeathEvent : IGameEvent {
            public static DeathEvent Default { get; } = new DeathEvent();
        }
    }
}