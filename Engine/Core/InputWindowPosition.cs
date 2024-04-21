using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Engine.Core
{
    public static partial class Input
    {
        [DllImport("kernel32")]
        public static extern IntPtr GetConsoleWindow();

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

        public static Bounds GetWindowBounds() =>
            new Bounds(new []{GetWindowLeftUpCornerPosition(), GetWindowRightDownCornerPosition()});

        private static Rect GetWindowRect()
        {
            if (!GetWindowRect(Process.GetCurrentProcess().MainWindowHandle, out Rect rect))
            {
                throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
            }

            return rect;
        }

        public static Vector2 GetWindowLeftUpCornerPosition()
        {
            var rect = GetWindowRect();
            return new Vector2(rect.Left, rect.Top);
        }

        public static Vector2 GetWindowRightDownCornerPosition()
        {
            var rect = GetWindowRect();
            return new Vector2(rect.Right, rect.Bottom);
        }
    }
}