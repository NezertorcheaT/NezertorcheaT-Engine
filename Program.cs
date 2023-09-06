using System;

namespace Console_Engine
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(GameConfig.GetData().ToString());
            Console.ReadKey();
        }
    }
}