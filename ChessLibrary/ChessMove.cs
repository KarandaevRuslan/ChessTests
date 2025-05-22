using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class ChessMove
    {
        public ChessField From { get; private set; }
        public ChessField To { get; private set; }
        public IChessman MovingPiece { get; private set; }
        public IChessman CapturedPiece { get; private set; }
        public bool IsCapture => CapturedPiece != null;
        public bool IsEnPassant { get; private set; }
        public bool IsCastling { get; private set; }
        public EChessmanType? PromotionPiece { get; private set; }

        public ChessMove(ChessField from, ChessField to, IChessman movingPiece,
                         IChessman capturedPiece = null, bool isEnPassant = false,
                         bool isCastling = false, EChessmanType? promotionPiece = null)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            MovingPiece = movingPiece ?? throw new ArgumentNullException(nameof(movingPiece));
            CapturedPiece = capturedPiece;
            IsEnPassant = isEnPassant;
            IsCastling = isCastling;
            PromotionPiece = promotionPiece;
        }

        public string AsString()
        {
            if (IsCastling)
                return To.Col == 'g' ? "O-O" : "O-O-O";

            string result = "";

            // Добавляем символ фигуры (кроме пешки)
            if (MovingPiece.Type != EChessmanType.Pawn)
            {
                result += GetPieceSymbol(MovingPiece.Type);
            }

            // Для пешки при взятии добавляем колонку отправления
            if (MovingPiece.Type == EChessmanType.Pawn && IsCapture)
            {
                result += From.Col;
            }

            // Добавляем символ взятия
            if (IsCapture || IsEnPassant)
            {
                result += "x";
            }

            // Добавляем поле назначения
            result += To.ToString();

            // Добавляем специальные символы
            if (IsEnPassant)
            {
                result += " e.p.";
            }

            if (PromotionPiece.HasValue)
            {
                result += "=" + GetPieceSymbol(PromotionPiece.Value);
            }

            return result;
        }

        private string GetPieceSymbol(EChessmanType type)
        {
            return type switch
            {
                EChessmanType.King => "K",
                EChessmanType.Queen => "Q",
                EChessmanType.Rook => "R",
                EChessmanType.Bishop => "B",
                EChessmanType.Knight => "N",
                EChessmanType.Pawn => "",
                _ => ""
            };
        }
    }
}
