using System;
using System.Threading.Tasks;
using Engine.Components;

namespace Engine
{
    internal static class Program
    {
        public static async void DrawCycle(Hierarchy h)
        {
            var d = GameConfig.GetData();
            SymbolMatrix m;

            while (true)
            {
                await Task.Delay((int) (1.0 / (double) d.FPS * 1000.0));
                Console.SetWindowSize((int) d.WIDTH + 2, (int) d.HEIGHT + 2);
                m = new SymbolMatrix(d.WIDTH, d.HEIGHT);
                var gcd = GameConfig.GetData();

                foreach (IRenderer obj in GameObject.FindAllTypes<Drawer>(h))
                {
                    try
                    {
                        obj.OnDraw(m, gcd);
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e, "drawing error");
                    }
                }

                Console.Clear();
                Console.Write(m.ToString());
            }
        }

        public static void Main(string[] args)
        {
            Logger.Initialise();
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                if (!Logger.IsSessionExist) return;

                Logger.Log(sender ?? "null", "closing event sender");
                Logger.Stop();
            };

            var d = GameConfig.GetData();
            var m = new SymbolMatrix(d.WIDTH, d.HEIGHT);

            Hierarchy h;
            try
            {
                h = Scene.GenerateMap(d.MAP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
                return;
            }

            Logger.Log(d.ToString());

            foreach (IGameObjectStartable obj in h.Objs)
            {
                try
                {
                    obj.Start();
                }
                catch (Exception e)
                {
                    Logger.Log(e, "start error");
                }
            }

            DrawCycle(h);

            while (true)
            {
                foreach (IGameObjectUpdatable obj in h.Objs)
                {
                    try
                    {
                        obj.Update();
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e, "update error");
                    }
                }
            }

            Logger.Stop();
            Console.ReadKey();
        }
    }
}