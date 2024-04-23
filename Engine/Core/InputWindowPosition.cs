using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine.Core
{
    public static partial class Input
    {
        [DllImport("kernel32")]
        private static extern IntPtr GetConsoleWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport(@"user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        public static Bounds GetWindowBounds()
        {
            var rect = GetWindowRect();
            return new Bounds(new[] {new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Bottom)});
        }

        public static Bounds GetWindowFieldBounds()
        {
            var rect = GetWindowFieldRect();
            return new Bounds(new[] {new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Bottom)});
        }


        private static Rect GetWindowRect()
        {
            if (!GetWindowRect(Process.GetCurrentProcess().MainWindowHandle, out Rect rect))
            {
                throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
            }

            return rect;
        }

        private static Rect GetWindowFieldRect()
        {
            var rect = GetWindowRect();

            rect.Top += 31;
            rect.Left -= 1;
            rect.Right += 18;
            rect.Bottom -= 18;

            return rect;
        }

        public static Vector2 GetWindowFieldLeftUpCornerPosition()
        {
            var rect = GetWindowFieldRect();
            return new Vector2(rect.Left, rect.Top);
        }

        public static Vector2 GetWindowLeftUpCornerPosition()
        {
            var rect = GetWindowRect();
            return new Vector2(rect.Left, rect.Top);
        }

        public static Vector2 GetWindowFieldRightDownCornerPosition()
        {
            var rect = GetWindowFieldRect();
            return new Vector2(rect.Right, rect.Bottom);
        }

        public static Vector2 GetWindowRightDownCornerPosition()
        {
            var rect = GetWindowRect();
            return new Vector2(rect.Right, rect.Bottom);
        }
    }
}