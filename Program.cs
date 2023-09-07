using System;

namespace Console_Engine
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Logger.Initialise();
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                if (!Logger.IsSessionExist) return;

                Logger.Log(sender ?? "null", "closing event sender");
                Logger.Stop();
            };

            try
            {
                MainDo(args);
            }
            catch (Exception e)
            {
                Logger.Log(e, e.GetType().Name);
            }

            Logger.Stop();
        }

        private static void MainDo(string[] args)
        {
            var d = GameConfig.GetData();
            Logger.Log(d.ToString());

            var m = new SymbolMatrix(d.WIDTH, d.HEIGHT);
            Console.WriteLine(m.ToString());
            Console.ReadKey();
        }
    }
}