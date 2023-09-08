using System;
using System.Numerics;

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
            var h = new Hierarchy();

            for (var i = 0; i < 10; i++)
            {
                GameObject.Instantiate($"ass{i}", "none", 0, h).transform.LocalPosition =
                    new Vector2(RandomRange(10), RandomRange(10));
            }

            foreach (var obj in h.Objs)
            {
                var pos = obj.transform.Position;
                m.Draw(new Symbol {Character = 'S', Color = ConsoleColor.White},
                    new Size((uint) Math.Round(pos.X), (uint) Math.Round(pos.Y)));
            }

            Console.WriteLine(m.ToString());
            Console.ReadKey();
        }

        private static int RandomRange(int max = 100)
        {
            var rand = new Random();
            return rand.Next(max + 1);
        }
    }
}