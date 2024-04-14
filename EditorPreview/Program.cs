using System;
using Engine.Core;

namespace EditorPreview
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            Preview.Start();
            Preview.BuildMap();
            GameConfig.Data.MAP = args[0];
            Preview.ShowMap();

            Console.ReadKey();
        }
    }
}