using ChessLibrary;
using System;

namespace ChessTest
{
    /// <summary>
    /// Набор тестов для проверки логики пешки: движение, взятие, превращение, взятие на проходе и ошибки.
    /// </summary>
    [TestFixture]
    public class PawnTest
    {
        private ChessBoard board;
        private Pawn whitePawn;
        private Pawn blackPawn;

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
            whitePawn = new Pawn(new ChessField('2', 'e'), ESide.White);
            blackPawn = new Pawn(new ChessField('7', 'e'), ESide.Black);

            board.PlacePiece(whitePawn, whitePawn.Position);
            board.PlacePiece(blackPawn, blackPawn.Position);
        }

        [Test]
        public void Pawn_OneStepForward_ShouldBeValid()
        {
            var targetField = new ChessField('3', 'e');
            var move = whitePawn.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("e3"));
            Assert.That(move.IsCapture, Is.False);
            Assert.That(whitePawn.GetPosition(), Is.EqualTo(targetField));
        }

        [Test]
        public void Pawn_TwoStepsFromStart_ShouldBeValid()
        {
            var targetField = new ChessField('4', 'e');
            var move = whitePawn.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("e4"));
            Assert.That(move.IsCapture, Is.False);
        }

        [Test]
        public void Pawn_TwoStepsAfterMoving_ShouldThrowException()
        {
            // Сначала делаем один ход
            whitePawn.GoToPosition(new ChessField('3', 'e'), board);
            board.ExecuteMove(new ChessMove(whitePawn.Position, new ChessField('3', 'e'), whitePawn));

            // Теперь пытаемся сходить на два поля
            Assert.Throws<InvalidOperationException>(() =>
                whitePawn.GoToPosition(new ChessField('5', 'e'), board));
        }

        [Test]
        public void Pawn_DiagonalCapture_ShouldBeValid()
        {
            var enemyPawn = new Pawn(new ChessField('3', 'd'), ESide.Black);
            board.PlacePiece(enemyPawn, enemyPawn.Position);

            var targetField = new ChessField('3', 'd');
            var move = whitePawn.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("exd3"));
            Assert.That(move.IsCapture, Is.True);
            Assert.That(move.CapturedPiece, Is.EqualTo(enemyPawn));
        }

        [Test]
        public void Pawn_DiagonalMoveWithoutCapture_ShouldThrowException()
        {
            var targetField = new ChessField('3', 'd');
            Assert.Throws<InvalidOperationException>(() =>
                whitePawn.GoToPosition(targetField, board));
        }

        [Test]
        public void Pawn_ForwardMoveBlocked_ShouldThrowException()
        {
            var blockingPiece = new Pawn(new ChessField('3', 'e'), ESide.Black);
            board.PlacePiece(blockingPiece, blockingPiece.Position);

            Assert.Throws<InvalidOperationException>(() =>
                whitePawn.GoToPosition(new ChessField('3', 'e'), board));
        }

        [Test]
        public void Pawn_EnPassantCapture_ShouldBeValid()
        {
            // Устанавливаем белую пешку на 5 ряду
            var whitePawnOnFifth = new Pawn(new ChessField('5', 'e'), ESide.White);
            whitePawnOnFifth.MarkAsMoved();
            board.PlacePiece(whitePawnOnFifth, whitePawnOnFifth.Position);

            // Черная пешка делает ход на два поля рядом с белой
            var blackPawnForEnPassant = new Pawn(new ChessField('7', 'd'), ESide.Black);
            board.PlacePiece(blackPawnForEnPassant, blackPawnForEnPassant.Position);

            var enemyMove = blackPawnForEnPassant.GoToPosition(new ChessField('5', 'd'), board);
            board.ExecuteMove(enemyMove);

            // Теперь белая пешка может взять на проходе
            var targetField = new ChessField('6', 'd');
            var move = whitePawnOnFifth.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("exd6 e.p."));
            Assert.That(move.IsEnPassant, Is.True);
        }

        [Test]
        public void Pawn_PromotionToQueen_ShouldBeValid()
        {
            var pawnNearPromotion = new Pawn(new ChessField('7', 'e'), ESide.White);
            pawnNearPromotion.MarkAsMoved();
            board.PlacePiece(pawnNearPromotion, pawnNearPromotion.Position);

            var targetField = new ChessField('8', 'e');
            var move = pawnNearPromotion.GoToPosition(targetField, board);

            Assert.That(move.AsString(), Is.EqualTo("e8=Q"));
            Assert.That(move.PromotionPiece, Is.EqualTo(EChessmanType.Queen));
        }

        [Test]
        public void Pawn_BackwardMove_ShouldThrowException()
        {
            var targetField = new ChessField('1', 'e');
            Assert.Throws<InvalidOperationException>(() =>
                whitePawn.GoToPosition(targetField, board));
        }
    }
}
