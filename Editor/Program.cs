using System;
using System.Diagnostics;
using Engine.Core;

namespace Editor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            GameConfig.GetData();
            
            Process.Start(new ProcessStartInfo("EditorPreview.exe")
                {
                    Arguments = GameConfig.Data.MAP,
                    UseShellExecute = true
                }
            );

            Console.ReadKey();
        }
    }
}