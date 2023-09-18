using System;
using System.Threading.Tasks;

namespace Engine
{
    internal static class Program
    {
        public static async void DrawCycle(Hierarchy h)
        {
            SymbolMatrix m;

            while (true)
            {
                await Task.Delay((int) (1.0 / (double) GameConfig.Data.FPS * 1000.0));
                Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);
                m = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);

                foreach (IRenderer obj in GameObject.FindAllTypes<Drawer>(h))
                {
                    try
                    {
                        obj.OnDraw(m);
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
            GameConfig.GetData();
            var m = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);

            Hierarchy h;
            try
            {
                h = Scene.GenerateMap(GameConfig.Data.MAP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
                return;
            }

            Logger.Log(GameConfig.Data.ToString());

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