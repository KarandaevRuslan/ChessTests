using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class Pawn : Chessman
    {
        public Pawn(ChessField position, ESide side) : base(EChessmanType.Pawn, position, side)
        {
        }

        public override ChessMove GoToPosition(ChessField targetField, ChessBoard board)
        {
            if (!IsValidMove(Position, targetField, board))
                throw new InvalidOperationException("Недопустимый ход пешки");

            var capturedPiece = board.GetPiece(targetField);
            bool isEnPassant = IsEnPassantCapture(targetField, board);
            bool isCapture = capturedPiece != null || isEnPassant;

            if (isEnPassant)
            {
                var capturedPawnField = new ChessField(Position.Row, targetField.Col);
                capturedPiece = board.GetPiece(capturedPawnField);
            }

            // Проверяем превращение пешки
            EChessmanType? promotionPiece = null;
            if ((Side == ESide.White && targetField.Row == '8') ||
                (Side == ESide.Black && targetField.Row == '1'))
            {
                promotionPiece = EChessmanType.Queen; // По умолчанию превращаем в ферзя
            }

            var move = new ChessMove(Position, targetField, this, capturedPiece,
                                    isEnPassant, false, promotionPiece);

            UpdatePosition(targetField);
            return move;
        }

        protected override bool IsValidMove(ChessField from, ChessField to, ChessBoard board)
        {
            if (from.Col == to.Col) // Движение вперед
            {
                int direction = Side == ESide.White ? 1 : -1;
                int rowDiff = to.Row - from.Row;

                // Одно поле вперед
                if (rowDiff == direction)
                {
                    return !board.IsBusy(to);
                }

                // Два поля вперед с начальной позиции
                if (rowDiff == 2 * direction && !HasMoved)
                {
                    return !board.IsBusy(to) && !board.IsBusy(new ChessField((char)(from.Row + direction), from.Col));
                }
            }
            else if (Math.Abs(from.Col - to.Col) == 1) // Взятие по диагонали
            {
                int direction = Side == ESide.White ? 1 : -1;
                int rowDiff = to.Row - from.Row;

                if (rowDiff == direction)
                {
                    // Обычное взятие
                    var targetPiece = board.GetPiece(to);
                    if (targetPiece != null && targetPiece.Side != Side)
                        return true;

                    // Взятие на проходе
                    return IsEnPassantCapture(to, board);
                }
            }

            return false;
        }

        private bool IsEnPassantCapture(ChessField to, ChessBoard board)
        {
            if (board.LastMove == null) return false;

            var lastMove = board.LastMove;

            // Последний ход должен быть ходом пешки противника на два поля
            if (lastMove.MovingPiece.Type != EChessmanType.Pawn ||
                lastMove.MovingPiece.Side == Side ||
                Math.Abs(lastMove.From.Row - lastMove.To.Row) != 2)
                return false;

            // Пешка противника должна быть рядом
            if (lastMove.To.Row != Position.Row || Math.Abs(lastMove.To.Col - Position.Col) != 1)
                return false;

            // Целевое поле должно быть "за" пешкой противника
            int direction = Side == ESide.White ? 1 : -1;
            return to.Col == lastMove.To.Col && to.Row == lastMove.To.Row + direction;
        }
    }
}
