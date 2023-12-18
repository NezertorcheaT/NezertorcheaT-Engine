using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine
{
    public class SymbolMatrix
    {
        private Symbol[][] _matrix;
        private readonly Vector2 _size;

        public SymbolMatrix(uint w, uint h)
        {
            _matrix = new Symbol[h][];
            for (var i = 0; i < _matrix.Length; i++)
            {
                _matrix[i] = new Symbol[w];
            }

            _size = new Vector2(h, w);

            for (var x = 0; x < _size.X; x++)
            {
                for (var y = 0; y < _size.Y; y++)
                {
                    _matrix[x][y] = new Symbol();
                }
            }
        }

        public void Draw(Symbol symbol, Vector2 pos)
        {
            _matrix[(int) Math.Round(Math.Clamp(pos.Y, 0, _size.Y))]
                    [(int) Math.Round(Math.Clamp(pos.X, 0, _size.X))] =
                symbol;
        }

        public override string ToString()
        {
            //var s = ConsoleColorsTable.ColorsTable[ConsoleColor.Reset];
            var s = new StringBuilder();
            for (var x = 0; x < _size.X; x++)
            {
                for (var y = 0; y < _size.Y; y++)
                {
                    //s += ConsoleColorsTable.ColorsTable[_matrix[x, y].Color] + _matrix[x, y].Character;
                    s.Append(_matrix[x][y].Character);
                }

                s.Append('\n');
            }

            s.Append(Symbol.ColorsTable[Symbol.ConsoleColor.Reset]);
            return s.ToString();
        }
    }

    public class Symbol
    {
        public char Character = '.';
        public ConsoleColor Color = ConsoleColor.Yellow;
        
        public static readonly Dictionary<ConsoleColor, string> ColorsTable = new Dictionary<ConsoleColor, string>()
        {
            {ConsoleColor.Reset, "\u001B[0m"},
            {ConsoleColor.White, "\u001B[37m"},
            {ConsoleColor.Yellow, "\u001B[33m"},
            {ConsoleColor.Red, "\u001B[31m"},
            {ConsoleColor.Black, "\u001B[30m"},
            {ConsoleColor.Blue, "\u001B[34m"},
            {ConsoleColor.Green, "\u001B[32m"},
            {ConsoleColor.Cyan, "\u001B[36m"},
            {ConsoleColor.Magenta, "\u001B[35m"},
        };
        
        public enum ConsoleColor
        {
            Black,
            Blue,
            Green,
            Cyan,
            Red,
            Magenta,
            Yellow,
            White,
            Reset,
        }
    }
}