using System;
using System.Text;
using System.Threading;
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
        public static void Start()
        {
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

            //Logger.Log(GameConfig.Data.ToString(), "Current Config");
            //Logger.Log(GameConfig.DefaultConfig, "Default Config");

            Hierarchy h;
            try
            {
                GameConfig.SetupHierarchy(() => HierarchyFactory.CreateHierarchy(GameConfig.Data.MAP,true));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
                Stop();
                return;
            }

            foreach (IGameObjectStartable obj in GameConfig.GameHierarchy.Objects)
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
        public static void DrawCycle()
        {
            if (GameConfig.Data.START_RESIZE_WINDOW)
                Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);
            while (IsWork)
            {
                if (GameConfig.Data.FPS > 0)
                    Thread.Sleep((int) (1000.0 / GameConfig.Data.FPS));

                if (GameConfig.Data.RESIZE_WINDOW)
                    Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);

                if (!GameConfig.Data.DRAW_PRIOIRITY && MainLoopWorking) continue;

                var matrix = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);

                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                Logger.Assert(GameConfig.GameHierarchy != null, "GameConfig.GameHierarchy != null");
                DrawLoopWorking = true;
                foreach (IRenderer obj in GameObject.FindAllTypes<IRenderer>(GameConfig.GameHierarchy))
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

                DrawLoopWorking = false;

                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(0, 0);
                    var stringBuilder = new StringBuilder();
                    /*                   
                    for (var x = 0; x < GameConfig.Data.WIDTH; x++)
                    {
                        for (var y = 0; y < GameConfig.Data.HEIGHT; y++)
                        {
                            stringBuilder.Append(matrix.Read(matrix.IFromPos(x, y)).Character);
                        }
                        stringBuilder.Append('\n');
                    }
                    Console.Write(stringBuilder.ToString());
                   */
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

        public static bool DrawLoopWorking { get; private set; }
        public static bool MainLoopWorking { get; private set; }

        /// <summary>
        /// Main update thread
        /// </summary>
        /// <param name="hierarchy">Scene to Main Updating</param>
        public static void MainLoop()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Logger.Log("Starting World Cycle");
            
            while (IsWork)
            {
                Thread.Yield();
                MainLoopWorking = false;

                if (GameConfig.Data.DRAW_PRIOIRITY && DrawLoopWorking) continue;

                foreach (IGameObjectUpdatable obj in GameConfig.GameHierarchy.Objects)
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
                
                MainLoopWorking = true;
                Time.SetDeltaTime(watch.Elapsed.TotalSeconds);
                watch.Restart();
            }

            Stop();
        }
    }
}