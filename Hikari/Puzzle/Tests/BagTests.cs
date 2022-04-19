using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;

namespace Hikari.Puzzle.Tests {
    public class BagTests {
        [Test]
        public void Size() {
            Assert.AreEqual(1, UnsafeUtility.SizeOf<Bag>());
        }

        [Test]
        public void TakeSingle() {
            var bag = new Bag();
            bag.Take(PieceKind.I);
            Assert.AreEqual("O T J L S Z 6", bag.ToString());
            bag = new Bag();
            bag.Take(PieceKind.O);
            Assert.AreEqual("I T J L S Z 6", bag.ToString());
            bag = new Bag();
            bag.Take(PieceKind.T);
            Assert.AreEqual("I O J L S Z 6", bag.ToString());
            bag = new Bag();
            bag.Take(PieceKind.J);
            Assert.AreEqual("I O T L S Z 6", bag.ToString());
            bag = new Bag();
            bag.Take(PieceKind.L);
            Assert.AreEqual("I O T J S Z 6", bag.ToString());
            bag = new Bag();
            bag.Take(PieceKind.S);
            Assert.AreEqual("I O T J L Z 6", bag.ToString());
            bag = new Bag();
            bag.Take(PieceKind.Z);
            Assert.AreEqual("I O T J L S 6", bag.ToString());
        }

        [Test]
        public void TakeMany() {
            var bag = new Bag();
            for (var i = 0; i < 7; i++) {
                bag.Take((PieceKind) i);
            }

            bag.Take(PieceKind.Z);
            bag.Take(PieceKind.T);
            Assert.AreEqual("I O J L S 5", bag.ToString());
        }
    }
}