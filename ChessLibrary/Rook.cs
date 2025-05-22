using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class Rook : Chessman
    {
        public Rook(ChessField position, ESide side) : base(EChessmanType.Rook, position, side)
        {
        }

        public override ChessMove GoToPosition(ChessField targetField, ChessBoard board)
        {
            if (!IsValidMove(Position, targetField, board))
                throw new InvalidOperationException("Недопустимый ход ладьи");

            var capturedPiece = board.GetPiece(targetField);
            var move = new ChessMove(Position, targetField, this, capturedPiece);

            UpdatePosition(targetField);
            return move;
        }

        protected override bool IsValidMove(ChessField from, ChessField to, ChessBoard board)
        {
            // Ладья ходит по горизонтали или вертикали
            if (from.Row != to.Row && from.Col != to.Col)
                return false;

            // Проверяем, что путь свободен
            if (!IsPathClear(from, to, board))
                return false;

            // Проверяем целевое поле
            var targetPiece = board.GetPiece(to);
            return targetPiece == null || targetPiece.Side != Side;
        }

        private bool IsPathClear(ChessField from, ChessField to, ChessBoard board)
        {
            int rowDirection = Math.Sign(to.Row - from.Row);
            int colDirection = Math.Sign(to.Col - from.Col);

            char currentRow = (char)(from.Row + rowDirection);
            char currentCol = (char)(from.Col + colDirection);

            while (currentRow != to.Row || currentCol != to.Col)
            {
                var field = new ChessField(currentRow, currentCol);
                if (board.IsBusy(field))
                    return false;

                currentRow = (char)(currentRow + rowDirection);
                currentCol = (char)(currentCol + colDirection);
            }

            return true;
        }
    }
}
