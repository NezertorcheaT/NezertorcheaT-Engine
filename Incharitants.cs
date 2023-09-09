using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace Engine
{
    public static class ReflectiveEnumerator
    {
        public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class => Assembly.GetAssembly(typeof(T))
            .GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));

        public static Vector2 Da2V2(this double[] arr) => new Vector2((float) arr[0], (float) arr[1]);
        public static Vector2 Da2V2(this float[] arr) => new Vector2(arr[0], arr[1]);
        public static Vector2 Da2V2(this int[] arr) => new Vector2(arr[0], arr[1]);
        public static Vector2 Da2V2(this uint[] arr) => new Vector2(arr[0], arr[1]);
        public static Vector2 Da2V2(this long[] arr) => new Vector2(arr[0], arr[1]);
    }
}