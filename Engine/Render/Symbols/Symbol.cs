using System;
using Engine.Core;

namespace Engine.Render.Symbols
{
    /// <summary>
    /// Colored character for rendering
    /// </summary>
    public class Symbol
    {
        public char Character = '.';
        public ConsoleColor Color = ConsoleColor.White;

        /// <summary>
        /// Character aspect
        /// </summary>
        public static float Aspect
        {
            get
            {
                var font = Input.GetConsoleFont();
                return font.Size.Y / font.Size.X;
            }
        }

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

        public static bool operator ==(Symbol a, Symbol b) => a.Equals(b);
        public static bool operator !=(Symbol a, Symbol b) => !a.Equals(b);

        protected bool Equals(Symbol other)
        {
            return Character == other.Character && Color == other.Color;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Symbol) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Character, (int) Color);
        }
    }
}