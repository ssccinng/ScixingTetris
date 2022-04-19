using System;
using UnityEngine;
using static Hikari.Puzzle.Visual.ConnectionDirection;

namespace Hikari.Puzzle.Visual {
    public static class CellVisualHelper {
        public static Color GetColor(PieceKind kind) {
            switch (kind) {
                case PieceKind.I:
                    return Color.cyan;
                case PieceKind.O:
                    return Color.yellow;
                case PieceKind.T:
                    return new Color(0.8f, 0f, 0.9f, 1);
                case PieceKind.J:
                    return Color.blue;
                case PieceKind.L:
                    return new Color(1f, 0.6f, 0.1f, 1);
                case PieceKind.S:
                    return Color.green;
                case PieceKind.Z:
                    return Color.red;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        public static readonly ConnectionDirection[][][] Connections = {
            new[] {
                new[] {
                    Right,
                    Left | Right,
                    Left | Right,
                    Left
                },
                new[] {
                    Up,
                    Down | Up,
                    Down | Up,
                    Down
                },
                new[] {
                    Right,
                    Left | Right,
                    Left | Right,
                    Left
                },
                new[] {
                    Up,
                    Down | Up,
                    Down | Up,
                    Down
                }
            },
            new[] {
                new[] {
                    Right | Up,
                    Right | Down,
                    Left | Up,
                    Left | Down
                },
                new[] {
                    Right | Up,
                    Right | Down,
                    Left | Up,
                    Left | Down
                },
                new[] {
                    Right | Up,
                    Right | Down,
                    Left | Up,
                    Left | Down
                },
                new[] {
                    Right | Up,
                    Right | Down,
                    Left | Up,
                    Left | Down
                }
            },
            new[] {
                new[] {
                    Right,
                    Left | Up | Right,
                    Left,
                    Down
                },
                new[] {
                    Up,
                    Down | Right | Up,
                    Left,
                    Down
                },
                new[] {
                    Up,
                    Right,
                    Left | Down | Right,
                    Left
                },
                new[] {
                    Up,
                    Right,
                    Left | Down | Up,
                    Down
                }
            },
            new[] {
                new[] {
                    Right | Up,
                    Left | Right,
                    Left,
                    Down
                },
                new[] {
                    Up,
                    Down | Up,
                    Down | Right,
                    Left
                },
                new[] {
                    Up,
                    Right,
                    Left | Right,
                    Left | Down
                },
                new[] {
                    Right,
                    Left | Up,
                    Down | Up,
                    Down
                }
            },
            new[] {
                new[] {
                    Right,
                    Left | Right,
                    Left | Up,
                    Down
                },
                new[] {
                    Right | Up,
                    Left,
                    Down | Up,
                    Down
                },
                new[] {
                    Up,
                    Down | Right,
                    Left | Right,
                    Left
                },
                new[] {
                    Up,
                    Down | Up,
                    Right,
                    Down | Left
                }
            },
            new[] {
                new[] {
                    Right,
                    Left | Up,
                    Down | Right,
                    Left
                },
                new[] {
                    Up,
                    Right | Up,
                    Left | Down,
                    Down
                },
                new[] {
                    Right,
                    Left | Up,
                    Down | Right,
                    Left
                },
                new[] {
                    Up,
                    Right | Up,
                    Left | Down,
                    Down
                }
            },
            new[] {
                new[] {
                    Right | Up,
                    Left,
                    Right,
                    Left | Down
                },
                new[] {
                    Up,
                    Down | Right,
                    Left | Up,
                    Down
                },
                new[] {
                    Right | Up,
                    Left,
                    Right,
                    Left | Down
                },
                new[] {
                    Up,
                    Down | Right,
                    Left | Up,
                    Down
                }
            }
        };
    }
}