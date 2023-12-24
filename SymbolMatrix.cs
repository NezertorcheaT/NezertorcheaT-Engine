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

        public int IFromPos(int x, float y) => IFromPos(new Vector2(x, y));
        public int IFromPos(float x, int y) => IFromPos(new Vector2(x, y));
        public int IFromPos(float x, float y) => IFromPos(new Vector2(x, y));
        public int IFromPos(int x, int y) => IFromPos(new Vector2(x, y));

        public int IFromPos(Vector2 pos) => (int) Math.Round(Math.Clamp(pos.X - 1, 0, _size.Y)) +
                                            (int) _size.Y *
                                            (int) Math.Round(Math.Clamp(pos.Y - 1, 0, _size.X));

        public Symbol Read(Vector2 pos) => _matrix[IFromPos(pos)];
        public Symbol Read(int pos) => _matrix[pos];

        public void Draw(Symbol symbol, Vector2 pos) => _matrix[IFromPos(pos)] = symbol;
    }
}