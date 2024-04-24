using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Engine.Components;
using Engine.Render.Symbols;

namespace Engine.Core
{
    public static partial class Input
    {
        /// <summary>
        /// Converts position from screen space to console symbol space
        /// </summary>
        /// <param name="screenPosition">position of point on screen</param>
        /// <returns></returns>
        public static Vector2 ScreenToConsolePosition(Vector2 screenPosition)
        {
            var v = screenPosition;
            var bounds = GetWindowFieldBounds();
            var font = GetConsoleFont();
            //v = bounds.Clamp(v);
            v -= bounds.LeftDown;
            v.X /= font.Size.X;
            v.Y /= font.Size.Y;
            return v;
        }

        /// <summary>
        /// Converts position from screen space to console symbol space
        /// </summary>
        /// <param name="consolePosition">position of symbol in console</param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Vector2 ConsoleToWorldPosition(Vector2 consolePosition, Camera camera)
        {
            consolePosition.X /= Symbol.Aspect;
            consolePosition -= camera.Offset;
            consolePosition.Multiply(new Vector2(1, -1));
            consolePosition += camera.transform.Position.Multiply(new Vector2(1, -1));
            
            return consolePosition.Multiply(new Vector2(1, -1));
        }

        public static Vector2 ConsoleToWorldPosition(Vector2 consolePosition)
        {
            consolePosition.X /= Symbol.Aspect;
            consolePosition.Multiply(new Vector2(1, -1));
            
            return consolePosition.Multiply(new Vector2(1, -1));
        }

        /// <summary>
        /// Get properties of console font
        /// </summary>
        /// <returns></returns>
        public static ConsoleFont GetConsoleFont()
        {
            var t = new _CONSOLE_FONT_INFOEX();
            if (GetCurrentConsoleFontEx(GetStdHandle(-11), false, t))
            {
                return new ConsoleFont(t.FontIndex, new Vector2(t.dwFontSize.X, t.dwFontSize.Y), t.FontFamily,
                    t.FontWeight, t.FaceName);
            }

            Logger.Log(Marshal.GetLastWin32Error(), "Last Win32 error");
            return null;
        }

        public class ConsoleFont
        {
            public int Index { get; }
            public Vector2 Size { get; }
            public int Family { get; }
            public int Weight { get; }
            public string Name { get; }

            public ConsoleFont(
                int Index,
                Vector2 Size,
                int Family,
                int Weight,
                string Name
            )
            {
                this.Index = Index;
                this.Size = Size;
                this.Family = Family;
                this.Weight = Weight;
                this.Name = Name;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class _CONSOLE_FONT_INFOEX
        {
            public int cbSize;

            public _CONSOLE_FONT_INFOEX()
            {
                cbSize = Marshal.SizeOf(typeof(_CONSOLE_FONT_INFOEX));
            }

            public int FontIndex;
            public _COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct _COORD
        {
            public short X;
            public short Y;
        }

        [DllImport(@"Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCurrentConsoleFontEx(
            IntPtr hConsoleOutput,
            [MarshalAs(UnmanagedType.Bool)] bool bMaximumWindow,
            [In, Out] _CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx
        );
    }
}