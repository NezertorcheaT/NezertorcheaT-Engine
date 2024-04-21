using System;
using Engine.Core;
using Engine.Scene;

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
            Preview.BuildMap(args[0]);
            GameConfig.Data.MAPS[0] = args[0];
            Preview.ShowMap();

            Console.ReadKey();
        }
    }
}