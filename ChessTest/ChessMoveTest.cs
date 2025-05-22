using ChessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTest
{
    /// <summary>
    /// Набор тестов для проверки корректности шахматной нотации и класса ChessMove.
    /// </summary>
    [TestFixture]
    public class ChessMoveTests
    {
        [Test]
        public void ChessMove_SimpleMove_ShouldReturnCorrectNotation()
        {
            var from = new ChessField('1', 'a');
            var to = new ChessField('1', 'h');
            var rook = new Rook(from, ESide.White);

            var move = new ChessMove(from, to, rook);

            Assert.That("Rh1".Equals(move.AsString()));
        }

        [Test]
        public void ChessMove_CaptureMove_ShouldReturnCorrectNotation()
        {
            var from = new ChessField('1', 'a');
            var to = new ChessField('1', 'h');
            var rook = new Rook(from, ESide.White);
            var capturedPiece = new Pawn(to, ESide.Black);

            var move = new ChessMove(from, to, rook, capturedPiece);

            Assert.That(move.AsString(), Is.EqualTo("Rxh1"));
        }

        [Test]
        public void ChessMove_PawnPromotion_ShouldReturnCorrectNotation()
        {
            var from = new ChessField('7', 'e');
            var to = new ChessField('8', 'e');
            var pawn = new Pawn(from, ESide.White);

            var move = new ChessMove(from, to, pawn, null, false, false, EChessmanType.Queen);

            Assert.That(move.AsString(), Is.EqualTo("e8=Q"));
        }
    }
}
