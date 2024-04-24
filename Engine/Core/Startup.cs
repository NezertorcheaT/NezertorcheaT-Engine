using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Engine.Components;
using Engine.Render.Symbols;
using Engine.Scene;

namespace Engine.Core
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
            static IEnumerable<Hierarchy> SetupHierarchies()
            {
                foreach (var map in GameConfig.Data.MAPS)
                {
                    var her = HierarchyFactory.CreateHierarchy(map, true);
                    try
                    {
                        StaticContainersFactory.CreateStaticContainers(her);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Logger.Log(e, "Static Containers creation error");
                        Stop();
                        continue;
                    }

                    yield return her;
                }
            }

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

            try
            {
                GameConfig.SetupHierarchy(SetupHierarchies);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
                Stop();
                return;
            }

            GameConfig.SetupRenderFeature();
            Logger.Log(GameConfig.RenderFeature.GetType().FullName, "render feature");

            foreach (IGameObjectStartable obj in GameConfig.SceneManager.CurrentHierarchy.Objects)
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
            
            foreach (var hierarchy in (GameConfig.SceneManager as IGameConfigSceneManager).Hierarchies)
            {
                try
                {
                    Logger.Log($"\n{HierarchyFactory.SaveHierarchy(hierarchy)}", "Export Hierarchy");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Logger.Log(e, "Export Hierarchy error");
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

                Logger.Assert(GameConfig.SceneManager.CurrentHierarchy != null, "GameConfig.GameHierarchy != null");
                Logger.Assert(GameConfig.RenderFeature != null, "GameConfig.RenderFeature != null");
                DrawLoopWorking = true;

                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (IRenderer obj in GameObject.FindAllTypes<IRenderer>(GameConfig.SceneManager.CurrentHierarchy))
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
                    GameConfig.RenderFeature.RenderProcedure(matrix);
                }
                catch (Exception e)
                {
                    Logger.Log(e, "drawing error");
                }

                DrawLoopWorking = false;
            }
        }

        public static bool DrawLoopWorking { get; private set; }
        public static bool MainLoopWorking { get; private set; }

        /// <summary>
        /// Main update thread
        /// </summary>
        public static void MainLoop()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Logger.Log("Starting World Cycle");

            while (IsWork)
            {
                Thread.Yield();
                MainLoopWorking = false;

                if (GameConfig.Data.DRAW_PRIOIRITY && DrawLoopWorking) continue;

                foreach (IGameObjectUpdatable obj in GameConfig.SceneManager.CurrentHierarchy.Objects)
                {
                    try
                    {
                        if (!(obj as GameObject)!.active) continue;
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

        /// <summary>
        /// Main update thread
        /// </summary>
        public static void FixedLoop()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Logger.Log("Starting Fixed Cycle");

            while (IsWork)
            {
                foreach (IGameObjectFixedUpdatable obj in GameConfig.SceneManager.CurrentHierarchy.Objects)
                {
                    try
                    {
                        if (!(obj as GameObject)!.active) continue;
                        obj.FixedUpdate();
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e, "update error");
                    }
                }

                var invDt = (int) ((GameConfig.GetData().FIXED_REPETITIONS - watch.Elapsed.TotalSeconds) * 1000);
                if (invDt > 0)
                    Thread.Sleep(invDt);

                watch.Restart();
            }

            Stop();
        }
    }
}