using System;
using System.Numerics;
using ConsoleEngine.Components;
using ConsoleEngine.IO;

namespace ConsoleEngine.Symbols
{
    /// <summary>
    /// Screen to render
    /// </summary>
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


        /// <summary>
        /// Convert from 2D Vector to array position
        /// </summary>
        /// <param name="x">X coordinate of 2D Vector position in matrix space</param>
        /// <param name="y">Y coordinate of 2D Vector position in matrix space</param>
        /// <returns></returns>
        public int IFromPos(int x, float y) => IFromPos(new Vector2(x, y));

        /// <summary>
        /// Convert from 2D Vector to array position
        /// </summary>
        /// <param name="x">X coordinate of 2D Vector position in matrix space</param>
        /// <param name="y">Y coordinate of 2D Vector position in matrix space</param>
        /// <returns></returns>
        public int IFromPos(float x, int y) => IFromPos(new Vector2(x, y));

        /// <summary>
        /// Convert from 2D Vector to array position
        /// </summary>
        /// <param name="x">X coordinate of 2D Vector position in matrix space</param>
        /// <param name="y">Y coordinate of 2D Vector position in matrix space</param>
        /// <returns></returns>
        public int IFromPos(float x, float y) => IFromPos(new Vector2(x, y));

        /// <summary>
        /// Convert from 2D Vector to array position
        /// </summary>
        /// <param name="x">X coordinate of 2D Vector position in matrix space</param>
        /// <param name="y">Y coordinate of 2D Vector position in matrix space</param>
        /// <returns></returns>
        public int IFromPos(int x, int y) => IFromPos(new Vector2(x, y));

        /// <summary>
        /// Convert from 2D Vector to array position
        /// </summary>
        /// <param name="pos">2D Vector position in matrix space</param>
        /// <returns></returns>
        public int IFromPos(Vector2 pos)
        {
            if (pos.X < 0 || pos.X >= _size.Y || pos.Y < 0 ||
                pos.Y >= _size.X) return _matrix.Length - 1;
            return (int) Math.Round(Math.Clamp(pos.X, 0, _size.Y - 1)) +
                   (int) _size.Y *
                   (int) Math.Round(Math.Clamp(pos.Y, 0, _size.X - 1));
        }

        /// <summary>
        /// Get Symbol at 2D Vector position of matrix
        /// </summary>
        /// <param name="pos">2D Vector position in matrix space</param>
        /// <returns></returns>
        public Symbol Read(Vector2 pos) => Read(IFromPos(pos));

        /// <summary>
        /// Get Symbol at array position of matrix
        /// </summary>
        /// <param name="pos">Array position, to get use IFromPos</param>
        /// <returns></returns>
        public Symbol Read(int pos) => _matrix[pos];

        /// <summary>
        /// Set Symbol at 2D Vector position of matrix
        /// </summary>
        /// <param name="symbol">Symbol to set</param>
        /// <param name="pos">2D Vector position in matrix space</param>
        public void Draw(Symbol symbol, Vector2 pos) => Draw(symbol, IFromPos(pos));

        /// <summary>
        /// Set Symbol at 2D Vector position of matrix
        /// </summary>
        /// <param name="symbol">Symbol to set</param>
        /// <param name="pos">Array position, to get use IFromPos</param>
        public void Draw(Symbol symbol, int pos) => _matrix[pos] = symbol;


        /// <summary>
        /// Convert world 2D Vector position to matrix space position
        /// </summary>
        /// <param name="pos">2D Vector world position, will be converted to matrix space</param>
        /// <param name="camera">Camera that renders</param>
        /// <param name="extends">If true, "pos" can be outside of matrix bounds</param>
        /// <returns>Is the positioning successful? If false, then "pos" will remain original</returns>
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