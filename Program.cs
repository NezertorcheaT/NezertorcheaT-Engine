using System;
using System.Threading.Tasks;

namespace Engine
{
    internal static class Program
    {
        private static async void DrawCycle(Hierarchy h)
        {
            SymbolMatrix m;

            while (true)
            {
                if (GameConfig.Data.FPS > 0)
                    await Task.Delay((int) (1000 / GameConfig.Data.FPS));
                
                Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);
                m = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);

                foreach (IRenderer obj in GameObject.FindAllTypes<IRenderer>(h))
                {
                    try
                    {
                        obj.OnDraw(m);
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e, "drawing error");
                        continue;
                    }

                    if (GameConfig.Data.LOG_DRAWCALLS)
                        Logger.Log($"{obj.gameObject}: {obj}, {obj.gameObject.transform.Position}", "drawcall");
                }

                try
                {
                    Console.SetCursorPosition(0,0);
                    Console.Write(m.ToString());
                }
                catch (Exception e)
                {
                    Logger.Log(e, "drawing error");
                }
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

            try
            {
                GameConfig.GetData();
                var m = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "game config error");
                Logger.Stop();
                return;
            }

            Hierarchy h;
            try
            {
                h = Scene.GenerateMap(GameConfig.Data.MAP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
                Logger.Stop();
                return;
            }

            Logger.Log(GameConfig.Data.ToString());
            Logger.Log(GameConfig.DefaultConfig);

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
            MainLoop(h);

            Logger.Stop();
            Console.ReadKey();
        }

        private static void MainLoop(Hierarchy h)
        {
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
        }
    }
}