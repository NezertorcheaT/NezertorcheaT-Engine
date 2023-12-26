using System;

namespace ConsoleEngine.Symbols
{
    public class Symbol
    {
        public char Character = '.';
        public ConsoleColor Color = ConsoleColor.White;

        public static readonly float FiveByEight = 52f / 25f;

        public Symbol()
        {
        }

        public Symbol(char character)
        {
            Character = character;
        }

        public Symbol(char character, ConsoleColor color)
        {
            Character = character;
            Color = color;
        }
    }
}