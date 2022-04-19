using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Hikari.Puzzle {
    public class Match : IDisposable {
        public bool Paused { get; set; }
        public bool ImmediateStart { get; set; }
        private readonly List<Game> games = new List<Game>();

        private bool ended;
        private float countdownTimer;
        private IDisposable updateSubscription;
        
        public event Action OnMatchStart;
        public event Action<int> OnCountDown;
        public event Action OnUpdate;
        public event Action<int> OnFinish;

        public Match(int gameCount) {
            if (gameCount <= 0) throw new ArgumentOutOfRangeException(nameof(gameCount),
                "A match needs at least one game to run");
            updateSubscription = Observable.EveryUpdate().Where(l => !Paused).Subscribe(Countdown);
            OnFinish += winner => {
                Debug.Log("Match finished winner:" + winner);
            };

            for (var i = 0; i < gameCount; i++) {
                var game = new Game(i, this);
                games.Add(game);
            }
        }

        public Game GetGame(int index) {
            return games[index];
        }

        private void Countdown(long ticks) {
            if (ticks == 60) OnCountDown?.Invoke(3);
            if (ticks == 120) OnCountDown?.Invoke(2);
            if (ticks == 180) OnCountDown?.Invoke(1);
            if (ImmediateStart && ticks > 70 || ticks == 240) {
                OnCountDown?.Invoke(0);
                OnMatchStart?.Invoke();
                updateSubscription.Dispose();
                updateSubscription = Observable.EveryUpdate().Where(l => !Paused).Subscribe(GameLoop);
            }
        }

        private void GameLoop(long ticks) {
            if (ended) return;
            
            OnUpdate?.Invoke();

            if (games.Count(game => !game.IsDead) <= (games.Count > 1 ? 1 : 0)) {
                OnFinish?.Invoke(games.FindIndex(g => !g.IsDead));
                ended = true;
            }
        }

        public void DistributeDamage(int sender, uint attack) {
            Debug.Log($"{attack} dmg from {sender}");
            if (games.Count < 2) return;
            foreach (var game in games.Where((g, i) => i != sender)) {
                game.AddDamage(attack);
            }
        }

        public void Dispose() {
            updateSubscription?.Dispose();
        }
    }
}