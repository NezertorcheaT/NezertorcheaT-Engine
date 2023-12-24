using System;
using System.Numerics;
using System.Text;

namespace Engine
{
    public class SymbolMatrix
    {
        private Symbol[] _matrix;
        private readonly Vector2 _size;

        public SymbolMatrix(uint w, uint h)
        {
            _matrix = new Symbol[w * h];
            _size = new Vector2(h, w);

            for (var i = 0; i < _matrix.Length; i++)
            {
                _matrix[i] = new Symbol();
            }
        }

        public void Draw(Symbol symbol, Vector2 pos)
        {
            _matrix[
                (int) Math.Round(Math.Clamp(pos.X-1, 0, _size.Y)) +
                (int) _size.Y *
                (int) Math.Round(Math.Clamp(pos.Y-1, 0, _size.X))
            ] = symbol;
        }

        public override string ToString()
        {
            //var s = ConsoleColorsTable.ColorsTable[ConsoleColor.Reset];
            var s = new StringBuilder();

            for (var i = 0; i < _matrix.Length; i++)
            {
                s.Append(_matrix[i].Character);
                if ((i + 1) % (int) _size.Y == 0 && i != 0)
                    s.Append('\n');
            }

            
            Console.ForegroundColor = ConsoleColor.White;
        
            return s.ToString();
        }
    }
}