using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Hikari.Puzzle.Tests {
    public class BoardTest {
        [Test]
        public void FiveNextOrder() {
            var board = new Board(5);
            Debug.Log(string.Concat(board.nextPieces.Select(n => n.ToString())));
            Assert.That(board.nextPieces.Distinct().Count() == board.nextPieces.Count(), Is.True);
        }

        [Test]
        public void TwelveNextOrder() {
            var board = new Board(12);
            Debug.Log(string.Concat(board.nextPieces.Select(n => n.ToString())));
            Assert.That(board.nextPieces.Take(7).Distinct().Count() == 7, Is.True);
        }
    }
}