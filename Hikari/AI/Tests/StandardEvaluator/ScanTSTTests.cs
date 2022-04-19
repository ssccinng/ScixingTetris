using Hikari.AI.Eval;
using NUnit.Framework;

namespace Hikari.AI.Tests.StandardEvaluator {
    public class ScanTSTTests {
        [Test]
        public void LeftNormal3Lines() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x..xxx....",
                "x...xxxxxx",
                "xxx.xxxxxx",
                "xx..xxxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(3, 2, 0, 3), result);
        }
        
        [Test]
        public void LeftNormal2LinesA() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x..xxx....",
                "x...xxxxxx",
                "xxx.xxxxx.",
                "xx..xxxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(2, 2, 0, 3), result);
        }
        
        [Test]
        public void LeftNormal2LinesB() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x..xxx....",
                "x...xxxxxx",
                "xxx.xxxxxx",
                "xx..xxxxx.",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(2, 2, 0, 3), result);
        }
        
        [Test]
        public void LeftNormal2LinesC() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x..xxx....",
                "x...xxxxxx",
                "xxx.xxxxxx",
                "xx..xxxxxx",
                "xxx.xxxxx."
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(2, 2, 0, 3), result);
        }
        
        [Test]
        public void LeftNormal1LineA() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x..xxx....",
                "x...xxxxxx",
                "xxx.xxxxxx",
                "xx..xxxxx.",
                "xxx.xxxxx."
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(1, 2, 0, 3), result);
        }
        
        [Test]
        public void LeftNormal1LineB() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x..xxx....",
                "x...xx....",
                "xxx.xxxxx.",
                "xx..xxxxxx",
                "xxx.xxxxx."
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(1, 2, 0, 3), result);
        }
        
        [Test]
        public void LeftNormal1LineC() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x..xxx....",
                "x...xx....",
                "xxx.xxxxx.",
                "xx..xxxxx.",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(1, 2, 0, 3), result);
        }
        
        [Test]
        public void LeftNoGuide() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "...xxx....",
                "....xx....",
                "xxx.xxxxxx",
                "xx..xxxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(3, 2, 0, 3), result);
        }
        
        [Test]
        public void LeftFailGuide() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "...xxx....",
                "x...xx....",
                "xxx.xxxxxx",
                "xx..xxxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.Null(result);
        }
        
        [Test]
        public void LeftNoAnchor() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x...xx....",
                "x...xx....",
                "xxx.xxxxxx",
                "xx..xxxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.Null(result);
        }
        
        [Test]
        public void LeftNoSkyAbove() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xx........",
                "x..xxx....",
                "x...xx....",
                "xxx.xxxxxx",
                "xx...xxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.Null(result);
        }
        
        [Test]
        public void RightNormal3Lines() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "xx..x.....",
                "x...xxxxxx",
                "x.xxxxxxxx",
                "x..xxxxxxx",
                "x.xxxxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(3, 0, 0, 1), result);
        }
        
        [Test]
        public void Imperial() {
            using var helper = new SimpleColBoardTestHelper(new[] {
                "x..xxx....",
                "x...xx....",
                "xxx.xxxxxx",
                "xx...xxxxx",
                "xxx.xxxxxx"
            });
            var result = Eval.StandardEvaluator.ScanTST(helper.Board);
            Assert.AreEqual( new TspinHole(2, 2, 0, 3), result);
        }
    }
}