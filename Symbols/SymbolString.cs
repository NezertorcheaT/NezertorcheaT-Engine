using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ConsoleEngine.Symbols
{
    /// <summary>
    /// Colored string for rendering
    /// </summary>
    public class SymbolString
    {
        public string String = "";
        public ConsoleColor Color = ConsoleColor.White;

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
        
        public SymbolString()
        {
        }

        public SymbolString(string str)
        {
            String = str;
        }

        public SymbolString(string str, ConsoleColor color)
        {
            String = str;
            Color = color;
        }

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

                matrix.Draw(symbol, pos + new Vector2(x, y));

                x++;
                if (symbol.Character != '\n') continue;
                y++;
                x = 0;
            }
        }
    }
    
    public static class SymbolStringFactory
    {
        public static SymbolString GetFromPath(string path)
        {
            return new SymbolString(new StreamReader(path).ReadToEnd() ?? "");
        }
    }
}