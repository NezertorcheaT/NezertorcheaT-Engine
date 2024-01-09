using ConsoleEngine.IO;

namespace ConsoleEngine
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Startup.Start();

            if (GameConfig.GameHierarchy == null) return;
            Startup.DrawCycle();
            Startup.MainLoop();

            Startup.Stop();
        }
    }
}