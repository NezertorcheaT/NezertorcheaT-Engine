using System;

namespace Engine
{
    public class SymbolMatrix
    {
        private Symbol[,] _matrix;
        private readonly Size _size;

        public SymbolMatrix(uint w, uint h)
        {
            _matrix = new Symbol[h, w];
            _size = new Size(h, w);

            for (var x = 0; x < _size.w; x++)
            {
                for (var y = 0; y < _size.h; y++)
                {
                    _matrix[x, y] = new Symbol();
                }
            }
        }

        public void Draw(Symbol symbol, Size pos)
        {
            _matrix[pos.h, pos.w] = symbol;
        }

        public override string ToString()
        {
            var s = "";
            for (var x = 0; x < _size.w; x++)
            {
                for (var y = 0; y < _size.h; y++)
                {
                    s += _matrix[x, y].Character;
                }

                s += '\n';
            }

            return s;
        }
    }

    public struct Size
    {
        public uint w;
        public uint h;

        public Size(uint w, uint h)
        {
            this.w = w;
            this.h = h;
        }
    }

    public class Symbol
    {
        public char Character = '.';
        public ConsoleColor Color = ConsoleColor.White;
    }
}