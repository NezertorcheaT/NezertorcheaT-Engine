﻿using System;
using System.Threading.Tasks;
using ConsoleEngine.Components;
using ConsoleEngine.Symbols;

namespace ConsoleEngine
{
    public static class Startup
    {
        public static void Start(out Hierarchy? hierarchy)
        {
            hierarchy = null;
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
                h = HierarchyFactory.CreateHierarchy(GameConfig.Data.MAP);
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

            hierarchy = h;
        }

        public static async void DrawCycle(Hierarchy h)
        {
            SymbolMatrix m;

            while (true)
            {
                if (GameConfig.Data.FPS > 0)
                    await Task.Delay((int) (1000 / GameConfig.Data.FPS));

                Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);
                m = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);

                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
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
                    for (var x = 0; x < GameConfig.Data.WIDTH; x++)
                    {
                        for (var y = 0; y < GameConfig.Data.HEIGHT; y++)
                        {
                            Console.SetCursorPosition(x, y);

                            var c = m.Read(m.IFromPos(x, y));
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

        public static void Stop()
        {
            Logger.Stop();
            Console.ReadKey();
        }

        public static void MainLoop(Hierarchy h)
        {
            while (true)
            {
                //if (GameConfig.Data.FPS > 0)
                //    await Task.Delay((int) (1000 / GameConfig.Data.FPS));
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