using ChessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTest
{
    /// <summary>
    /// Набор тестов для проверки логики ладьи: движение, взятие, блокировки и ошибки.
    /// </summary>
    [TestFixture]
    public class RookTest
    {
        private ChessBoard board;
        private Rook whiteRook;

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
            whiteRook = new Rook(new ChessField('1', 'a'), ESide.White);
            board.PlacePiece(whiteRook, whiteRook.Position);
        }

        [Test]
        public void Rook_HorizontalMove_ShouldBeValid()
        {
            var targetField = new ChessField('1', 'h');
            var move = whiteRook.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("Rh1"));
            Assert.That(move.IsCapture, Is.False);
            Assert.That(whiteRook.GetPosition(), Is.EqualTo(targetField));
        }

        [Test]
        public void Rook_VerticalMove_ShouldBeValid()
        {
            var targetField = new ChessField('8', 'a');
            var move = whiteRook.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("Ra8"));
            Assert.That(move.IsCapture, Is.False);
        }

        [Test]
        public void Rook_CaptureEnemyPiece_ShouldBeValid()
        {
            var enemyPiece = new Pawn(new ChessField('1', 'h'), ESide.Black);
            board.PlacePiece(enemyPiece, enemyPiece.Position);

            var targetField = new ChessField('1', 'h');
            var move = whiteRook.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("Rxh1"));
            Assert.That(move.IsCapture, Is.True);
            Assert.That(move.CapturedPiece, Is.EqualTo(enemyPiece));
        }

        [Test]
        public void Rook_DiagonalMove_ShouldThrowException()
        {
            var targetField = new ChessField('2', 'b');
            Assert.Throws<InvalidOperationException>(() =>
                whiteRook.GoToPosition(targetField, board));
        }

        [Test]
        public void Rook_MoveBlockedByOwnPiece_ShouldThrowException()
        {
            var ownPiece = new Pawn(new ChessField('1', 'c'), ESide.White);
            board.PlacePiece(ownPiece, ownPiece.Position);

            var targetField = new ChessField('1', 'h');
            Assert.Throws<InvalidOperationException>(() =>
                whiteRook.GoToPosition(targetField, board));
        }

        [Test]
        public void Rook_MoveBlockedByEnemyPiece_ShouldThrowException()
        {
            var enemyPiece = new Pawn(new ChessField('1', 'c'), ESide.Black);
            board.PlacePiece(enemyPiece, enemyPiece.Position);

            var targetField = new ChessField('1', 'h');
            Assert.Throws<InvalidOperationException>(() =>
                whiteRook.GoToPosition(targetField, board));
        }

        [Test]
        public void Rook_CaptureOwnPiece_ShouldThrowException()
        {
            var ownPiece = new Pawn(new ChessField('1', 'h'), ESide.White);
            board.PlacePiece(ownPiece, ownPiece.Position);

            var targetField = new ChessField('1', 'h');
            Assert.Throws<InvalidOperationException>(() =>
                whiteRook.GoToPosition(targetField, board));
        }

        [Test]
        public void Rook_LongVerticalMove_ShouldBeValid()
        {
            var targetField = new ChessField('7', 'a');
            var move = whiteRook.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("Ra7"));
            Assert.That(move.IsCapture, Is.False);
        }
    }
}
