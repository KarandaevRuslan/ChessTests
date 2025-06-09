using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// Абстрактный базовый класс для всех шахматных фигур.
    /// Содержит общие свойства и методы для фигур (тип, сторона, позиция, признак первого хода).
    /// </summary>
    public abstract class Chessman : IChessman
    {
        public EChessmanType Type { get; protected set; }
        public ESide Side { get; protected set; }
        public ChessField Position { get; protected set; }
        public bool HasMoved { get; private set; }

        protected Chessman(EChessmanType type, ChessField position, ESide side)
        {
            Type = type;
            Position = position;
            Side = side;
            HasMoved = false;
        }

        public ChessField GetPosition() => Position;

        public abstract ChessMove GoToPosition(ChessField targetField, ChessBoard board);

        protected abstract bool IsValidMove(ChessField from, ChessField to, ChessBoard board);

        public void MarkAsMoved()
        {
            HasMoved = true;
        }

        protected void UpdatePosition(ChessField newPosition)
        {
            Position = newPosition;
        }
    }
}
