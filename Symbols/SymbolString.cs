using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ConsoleEngine.Symbols
{
    public class SymbolString
    {
        public string String = "";

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

        public ConsoleColor Color = ConsoleColor.White;

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

        public static void PlaceAt(SymbolMatrix matrix, SymbolString symbolString, Vector2 pos)
        {
            var y = 0;
            var x = 0;

            foreach (var symbol in symbolString.SymbolArray)
            {
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
        public static SymbolString Get(string path)
        {
            return new SymbolString(new StreamReader(path).ReadToEnd() ?? "");
        }
    }
}