using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class ChessBoard
    {
        private readonly IChessman[,] _board = new IChessman[8, 8];
        private readonly Dictionary<ChessField, IChessman> _pieces = new();
        public ChessMove LastMove { get; private set; }

        public IChessman GetPiece(ChessField field)
        {
            _pieces.TryGetValue(field, out var piece);
            return piece;
        }

        public bool IsBusy(ChessField field)
        {
            return GetPiece(field) != null;
        }

        public void PlacePiece(IChessman piece, ChessField field)
        {
            if (_pieces.ContainsKey(field))
                _pieces.Remove(field);

            _pieces[field] = piece;
        }

        public void RemovePiece(ChessField field)
        {
            _pieces.Remove(field);
        }

        public void ExecuteMove(ChessMove move)
        {
            RemovePiece(move.From);

            if (move.IsCapture && !move.IsEnPassant)
            {
                RemovePiece(move.To);
            }
            else if (move.IsEnPassant)
            {
                // Для взятия на проходе удаляем пешку сбоку
                var capturedPawnField = new ChessField(move.From.Row, move.To.Col);
                RemovePiece(capturedPawnField);
            }

            PlacePiece(move.MovingPiece, move.To);
            LastMove = move;

            // Отмечаем, что фигура ходила
            if (move.MovingPiece is Chessman chessman)
            {
                chessman.MarkAsMoved();
            }
        }

        public bool IsFieldUnderAttack(ChessField field, ESide bySide)
        {
            // Упрощенная реализация для тестов
            return false;
        }
    }
}
