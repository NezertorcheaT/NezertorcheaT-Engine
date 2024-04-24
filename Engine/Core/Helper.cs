using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using Engine.Components.Physics;

namespace Engine.Core
{
    public static class Helper
    {
        public static IEnumerable<Type> GetEnumerableOfCurrentAssemblyType<T>() where T : class =>
            Assembly.GetAssembly(typeof(T))!
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));

        public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class
        {
            foreach (var myType in Assembly.GetAssembly(typeof(T))!.GetTypes())
            {
                if (
                    myType.IsClass &&
                    !myType.IsAbstract &&
                    (
                        myType.IsSubclassOf(typeof(T)) ||
                        myType.GetInterface(typeof(T).Name) != null
                    )
                )
                    yield return myType;
            }

            foreach (var myType in Assembly.GetEntryAssembly()!.GetTypes())
            {
                if (myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))) yield return myType;
            }
        }

        public static Vector2 Da2V2(this double[] arr) => new Vector2((float) arr[0], (float) arr[1]);
        public static Vector2 Da2V2(this float[] arr) => new Vector2(arr[0], arr[1]);
        public static Vector2 Da2V2(this int[] arr) => new Vector2(arr[0], arr[1]);
        public static Vector2 Da2V2(this uint[] arr) => new Vector2(arr[0], arr[1]);
        public static Vector2 Da2V2(this long[] arr) => new Vector2(arr[0], arr[1]);

        public static Vector2 Multiply(this Vector2 vec1, Vector2 vec2) =>
            new Vector2(vec1.X * vec2.X, vec1.Y * vec2.Y);

        public static double Repeat(double inp, double max)
        {
            if (inp <= max)
                return inp < 0 ? Repeat(max - inp, max) : inp;
            return inp > max ? Repeat(inp - max, max) : inp;
        }

        public static float Repeat(float inp, float max) => (float) Repeat((double) inp, max);
        public static double DegToRad => Math.PI / 180.0;
        public static float DegToRadFloat => (float) (Math.PI / 180.0);
        public static double RadToDeg => 180.0 / Math.PI;
        public static float RadToDegFloat => (float) (180.0 / Math.PI);
        
        public static Vector2 Normal(Vector2 a, Vector2 b) => new Vector2(-(b.Y - a.Y), b.X - a.X);
        public static Vector2 Floor(this Vector2 a) => new Vector2(MathF.Floor(a.X), MathF.Floor(a.Y));
        public static Vector2 Round(this Vector2 a) => new Vector2(MathF.Round(a.X), MathF.Round(a.Y));
        public static Vector2 Ceiling(this Vector2 a) => new Vector2(MathF.Ceiling(a.X), MathF.Ceiling(a.Y));

        public static Collision? CheckCollision(this SatTriangle triangle1, SatTriangle triangle2) =>
            SatTriangle.CheckCollision(triangle1, triangle2);


        public static string Multiply(this string s, int by)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < by; i++)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }

        public static int Lines(this StringBuilder sb) => sb.ToString().Lines();
        public static int Lines(this string str) => str.Split('\n').Length;

        public static int Wight(this StringBuilder sb) => Wight(sb.ToString());
        public static int Wight(this string str) => str.Split('\n').Select(c => c.Length).Prepend(int.MinValue).Max();

        public static char? At(this string str, int x, int y) => str.At(new Vector2(x, y));
        public static char? At(this string str, float x, float y) => str.At(new Vector2(x, y));

        public static char? At(this string str, Vector2 pos)
        {
            var ind = str.PosToInd(pos);
            if (ind >= 0) return str[ind];
            return null;
        }

        public static int PosToInd(this string str, Vector2 pos)
        {
            if (pos.X == 0 && pos.Y == 0) return 0;
            if (pos.X >= str.Wight() || pos.Y >= str.Lines()) return -1;
            if (pos.X < 0 || pos.Y < 0) return -1;

            return str.Split('\n').TakeWhile((t, j) => j != (int) pos.Y).Sum(t => t.Length + 1) + (int) pos.X;
        }

        public static string Insert(this string s1, string s2, Vector2 offset)
        {
            var sb = new StringBuilder();
            var s1Wight = s1.Wight();
            var s2Wight = s2.Wight();
            var s1Lines = s1.Lines();
            var s2Lines = s2.Lines();

            return sb.ToString();
        }

        public static string Offset(this string str, Vector2 offset)
        {
            var sb = new StringBuilder();
            var wight = str.Wight();
            var lines = str.Lines();

            for (var x = 0; x < offset.X + wight; x++)
            {
                for (var y = 0; y < offset.Y + lines; y++)
                {
                    if (x <= offset.X || y <= offset.Y)
                    {
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append(str.At(x, y));
                    }
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}