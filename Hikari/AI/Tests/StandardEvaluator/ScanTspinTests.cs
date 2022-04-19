using Hikari.AI.Eval;
using NUnit.Framework;

namespace Hikari.AI.Tests.StandardEvaluator {
    public class ScanTspinTests {
        [Test]
        public void LeftNormal2Lines() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xxx       ",
                "xx...xxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.AreEqual(new TspinHole(2, 2, 0, 2), result);
        }

        [Test]
        public void LeftNormal1LineA() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xxx       ",
                "xx...xxx.x",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.AreEqual(new TspinHole(1, 2, 0, 2), result);
        }

        [Test]
        public void LeftNormal1LineB() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xxx       ",
                "xx...xxxxx",
                "xxx.xxxx.x"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.AreEqual(new TspinHole(1, 2, 0, 2), result);
        }

        [Test]
        public void RightNormal2Lines() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xx  xx    ",
                "xx...xxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.AreEqual(new TspinHole(2, 2, 0, 2), result);
        }

        [Test]
        public void RightNormal1LineA() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xx  xx    ",
                "xx...xxx.x",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.AreEqual(new TspinHole(1, 2, 0, 2), result);
        }

        [Test]
        public void RightNormal1LineB() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xx  xx    ",
                "xx...xxxxx",
                "xxx.xxxx.x"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.AreEqual(new TspinHole(1, 2, 0, 2), result);
        }

        [Test]
        public void NotEnoughVerts() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xxx       ",
                "xx...xxxxx",
                "xxx..xxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.Null(result);
        }

        [Test]
        public void NoRoof() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xx.       ",
                "xx...xxxxx",
                "xxx..xxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.Null(result);
        }

        [Test]
        public void NoSkyAbove() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xxx.x.....",
                "xxxx      ",
                "xxx...xxxx",
                "xxxx..xxxx"
            });
            var result = Eval.StandardEvaluator.ScanTspin(helper.Board);
            Assert.Null(result);
        }
    }
}