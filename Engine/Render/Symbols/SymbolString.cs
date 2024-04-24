using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Engine.Core;

namespace Engine.Render.Symbols
{
    /// <summary>
    /// Colored string for rendering
    /// </summary>
    public class SymbolString
    {
        public string String;
        public ConsoleColor Color;

        /// <summary>
        /// Character enumerable
        /// </summary>
        public IEnumerable<Symbol> SymbolArray
        {
            get
            {
                foreach (var Char in String)
                {
                    yield return new Symbol(Char, Color);
                }
            }
        }


        private static IEnumerable<Vector2> BoundsPoints(string String)
        {
            for (int x = 0; x < String.Wight(); x++)
            {
                for (int y = 0; y < String.Length; y++)
                {
                    yield return new Vector2(x, y);
                }
            }
        }

        public Bounds Bounds
        {
            get
            {
                var b = new Bounds(BoundsPoints(String));
                b.Size += Vector2.One;
                return b;
            }
        }

        #region Constructors

        public SymbolString()
        {
            String = "";
            Color = ConsoleColor.White;
        }

        public SymbolString(string str)
        {
            String = str;
            Color = ConsoleColor.White;
        }

        public SymbolString(string str, ConsoleColor color)
        {
            String = str;
            Color = color;
        }

        public SymbolString(ConsoleColor color)
        {
            String = "";
            Color = color;
        }

        #endregion

        /// <summary>
        /// Render String in SymbolMatrix
        /// </summary>
        /// <param name="matrix">Matrix to render at</param>
        /// <param name="symbolString">String to render</param>
        /// <param name="pos">2D Vector position in matrix space</param>
        public static void PlaceAt(SymbolMatrix matrix, SymbolString symbolString, Vector2 pos)
        {
            var y = 0;
            var x = 0;

            foreach (var symbol in symbolString.SymbolArray)
            {
                if (symbol.Character == ' ')
                {
                    x++;
                    continue;
                }

                matrix.Draw(symbol.Character == ' ' ? new Symbol(' ', symbol.Color) : symbol, pos + new Vector2(x, y));

                x++;
                if (symbol.Character != '\n') continue;
                y++;
                x = 0;
            }
        }

        /// <summary>
        /// Insert SymbolString into another SymbolString
        /// </summary>
        /// <param name="symbolStringTo"></param>
        /// <param name="symbolStringFrom"></param>
        /// <param name="pos">2D Vector position in symbolStringTo space</param>
        public static void Insert(SymbolString symbolStringTo, SymbolString symbolStringFrom, Vector2 pos)
        {
            var y = 0;
            var x = 0;

            foreach (var symbol in symbolStringFrom.SymbolArray)
            {
                if (symbol.Character == ' ')
                {
                    x++;
                    continue;
                }

                symbolStringTo.Draw(symbol.Character, pos + new Vector2(x, y));

                x++;
                if (symbol.Character != '\n') continue;
                y++;
                x = 0;
            }
        }

        /// <summary>
        /// Insert Symbol into this SymbolString
        /// Throws away Symbol's color
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="pos">2D Vector position in this SymbolString space</param>
        public void Draw(Symbol symbol, Vector2 pos) => Draw(symbol.Character, pos);

        /// <summary>
        /// Insert char into this SymbolString
        /// </summary>
        /// <param name="character"></param>
        /// <param name="pos">2D Vector position in this SymbolString space</param>
        public void Draw(char character, Vector2 pos)
        {
            var strBuff = new StringBuilder();
            var x = 0;
            var y = 0;

            foreach (var symbol in SymbolArray)
            {
                x++;
                if (symbol.Character.Equals('\n'))
                {
                    x = 0;
                    y++;
                }

                if (y == (int) pos.Y)
                {
                    if (x == (int) pos.X)
                        strBuff.Append(character);
                    else if (x > (int) pos.X)
                    {
                        for (int i = 0; i < (int) pos.X - x; i++)
                        {
                            strBuff.Append(' ');
                        }

                        strBuff.Append(character);
                    }
                }
                else
                    strBuff.Append(symbol.Character);
            }

            String = strBuff.ToString();
        }

        protected bool Equals(SymbolString other)
        {
            return String == other.String && Color == other.Color;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SymbolString) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(String, (int) Color);
        }

        public static bool operator ==(SymbolString a, SymbolString b) => a.Equals(b);
        public static bool operator !=(SymbolString a, SymbolString b) => !a.Equals(b);
    }

    public static class SymbolStringFactory
    {
        public static SymbolString GetFromPath(string path)
        {
            return new SymbolString(new StreamReader(path).ReadToEnd() ?? "");
        }
    }
}