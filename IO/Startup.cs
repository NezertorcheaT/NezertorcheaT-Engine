using System;
using System.Threading.Tasks;
using ConsoleEngine.Components;
using ConsoleEngine.Scene;
using ConsoleEngine.Symbols;

namespace ConsoleEngine.IO
{
    public static class Startup
    {
        /// <summary>
        /// Is app work as intended
        /// </summary>
        public static bool IsWork { get; private set; }

        /// <summary>
        /// Full preparation of app to run
        /// </summary>
        /// <param name="hierarchy">Prepared scene</param>
        public static void Start(out Hierarchy? hierarchy)
        {
            hierarchy = null;
            Logger.Initialise();
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                IsWork = false;
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
                Stop();
                return;
            }

            Hierarchy h;
            try
            {
                h = HierarchyFactory.CreateHierarchy(GameConfig.Data.MAP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
                Stop();
                return;
            }

            Logger.Log(GameConfig.Data.ToString(), "Current Config");
            Logger.Log(GameConfig.DefaultConfig, "Default Config");

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

            hierarchy = h;
            IsWork = true;
        }

        /// <summary>
        /// Safe app stop
        /// </summary>
        public static void Stop()
        {
            IsWork = false;
            if (!Logger.IsSessionExist) return;
            Logger.Stop();
        }

        /// <summary>
        /// Rendering thread
        /// </summary>
        /// <param name="hierarchy">Scene to render</param>
        public static async void DrawCycle(Hierarchy hierarchy)
        {
            if (GameConfig.Data.START_RESIZE_WINDOW)
                Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);
            while (IsWork)
            {
                if (GameConfig.Data.FPS > 0)
                    await Task.Delay((int) (1000 / GameConfig.Data.FPS));

                if (GameConfig.Data.RESIZE_WINDOW)
                    Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);

                var matrix = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);

                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (IRenderer obj in GameObject.FindAllTypes<IRenderer>(hierarchy))
                {
                    try
                    {
                        obj.OnDraw(matrix);
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
                    for (var x = 0; x < GameConfig.Data.WIDTH; x++)
                    {
                        for (var y = 0; y < GameConfig.Data.HEIGHT; y++)
                        {
                            Console.SetCursorPosition(x, y);

                            var c = matrix.Read(matrix.IFromPos(x, y));
                            Console.ForegroundColor = c.Color;
                            Console.Write(c.Character);
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(0, 0);
                }
                catch (Exception e)
                {
                    Logger.Log(e, "drawing error");
                }
            }
        }

        /// <summary>
        /// Main update thread
        /// </summary>
        /// <param name="hierarchy">Scene to Main Updating</param>
        public static void MainLoop(Hierarchy hierarchy)
        {
            while (IsWork)
            {
                //if (GameConfig.Data.FPS > 0)
                //    await Task.Delay((int) (1000 / GameConfig.Data.FPS));
                foreach (IGameObjectUpdatable obj in hierarchy.Objs)
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