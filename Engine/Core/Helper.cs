using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
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

        public static double Repeat(double inp, double max)
        {
            if (inp <= max)
            {
                return inp < 0 ? Repeat(max - inp, max) : inp;
            }

            return inp > max ? Repeat(inp - max, max) : inp;
        }

        public static float Repeat(float inp, float max) => (float) Repeat((double) inp, max);
        public static double DegToRad => Math.PI / 180.0;
        public static float DegToRadFloat => (float) (Math.PI / 180.0);
        public static double RadToDeg => 180.0 / Math.PI;
        public static float RadToDegFloat => (float) (180.0 / Math.PI);
        public static Vector2 Normal(Vector2 a, Vector2 b) => new Vector2(-(b.Y - a.Y), b.X - a.X);

        public static Collision? CheckCollision(this SatTriangle triangle1, SatTriangle triangle2) =>
            SatTriangle.CheckCollision(triangle1, triangle2);
    }
}