using System;
using System.Numerics;

namespace Engine
{
    public class SymbolMatrix
    {
        private Symbol[,] _matrix;
        private readonly Vector2 _size;

        public SymbolMatrix(uint w, uint h)
        {
            _matrix = new Symbol[h, w];
            _size = new Vector2(h, w);

            for (var x = 0; x < _size.X; x++)
            {
                for (var y = 0; y < _size.Y; y++)
                {
                    _matrix[x, y] = new Symbol();
                }
            }
        }

        public void Draw(Symbol symbol, Vector2 pos)
        {
            _matrix[(int) Math.Round(Math.Clamp(pos.Y, 0, _size.Y-1)), (int) Math.Round(Math.Clamp(pos.X, 0, _size.X-1))] =
                symbol;
        }

        public override string ToString()
        {
            var s = "";
            for (var x = 0; x < _size.X; x++)
            {
                for (var y = 0; y < _size.Y; y++)
                {
                    s += _matrix[x, y].Character;
                }

                s += '\n';
            }

            return s;
        }
    }

    public class Symbol
    {
        public char Character = '.';
        public ConsoleColor Color = ConsoleColor.White;
    }
}