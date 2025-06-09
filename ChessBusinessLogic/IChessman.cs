using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// Интерфейс для шахматной фигуры. Определяет основные свойства и методы для всех фигур.
    /// </summary>
    public interface IChessman
    {
        ChessField GetPosition();
        ChessMove GoToPosition(ChessField targetField, ChessBoard board);
        EChessmanType Type { get; }
        ESide Side { get; }
        bool HasMoved { get; }
    }
}
