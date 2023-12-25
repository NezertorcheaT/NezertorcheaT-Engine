using System;
using System.Numerics;

namespace Engine
{
    public class SymbolMatrix
    {
        private Symbol[] _matrix;
        private readonly Vector2 _size;

        public SymbolMatrix(uint w, uint h)
        {
            _matrix = new Symbol[w * h + GameConfig.Data.DRAW_BUFFER_SIZE];
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

        public int IFromPos(Vector2 pos)
        {
            if (pos.X < 0 || pos.X >= _size.Y || pos.Y < 0 ||
                pos.Y >= _size.X) return _matrix.Length - 1;
            return (int) Math.Round(Math.Clamp(pos.X, 0, _size.Y - 1)) +
                   (int) _size.Y *
                   (int) Math.Round(Math.Clamp(pos.Y, 0, _size.X - 1));
        }

        public Symbol Read(Vector2 pos) => _matrix[IFromPos(pos)];
        public Symbol Read(int pos) => _matrix[pos];

        public void Draw(Symbol symbol, Vector2 pos) => _matrix[IFromPos(pos)] = symbol;


        public static bool WorldToSymbolMatrixPosition(ref Vector2 pos, Camera? camera, bool extends = false)
        {
            if (camera == null) return false;

            var camPos = camera.transform.Position;
            camPos.Y = -camPos.Y;

            var posYrew = new Vector2(pos.X, -pos.Y);

            var newPos = posYrew - camPos + camera.Offset.Da2V2();
            newPos.X *= Symbol.FiveByEight;

            if (newPos.X < 0 || newPos.X >= GameConfig.Data.WIDTH || newPos.Y < 0 ||
                newPos.Y >= GameConfig.Data.HEIGHT)
            {
                if (!extends) return false;
                pos = newPos;
                return true;
            }

            pos = newPos;
            return true;
        }
    }
}