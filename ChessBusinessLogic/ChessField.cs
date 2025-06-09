using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// Представляет координаты поля на шахматной доске (строка и колонка).
    /// Проверяет корректность координат при создании.
    /// </summary>
    public class ChessField
    {
        public char Row { get; private set; }
        public char Col { get; private set; }

        public ChessField(char row, char col)
        {
            if (row < '1' || row > '8' || col < 'a' || col > 'h')
                throw new ArgumentException("Недопустимые координаты поля");

            Row = row;
            Col = col;
        }

        public char GetRow() => Row;
        public char GetCol() => Col;

        public override bool Equals(object obj)
        {
            if (obj is ChessField field)
                return Row == field.Row && Col == field.Col;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public override string ToString()
        {
            return $"{Col}{Row}";
        }

        public bool IsValid()
        {
            return Row >= '1' && Row <= '8' && Col >= 'a' && Col <= 'h';
        }
    }
}
