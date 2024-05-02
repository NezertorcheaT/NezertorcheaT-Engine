using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Engine.Components;
using Engine.Render.Symbols;
using Engine.Scene;

[assembly: InternalsVisibleTo("Editor")]
[assembly: InternalsVisibleTo("EditorPreview")]
[assembly: InternalsVisibleTo("EngineTests")]

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
                    var her = GameConfig.HierarchyFactory.CreateHierarchy(map, true);
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

            Input.ConsoleFontUpdate();

            Logger.Initialise();
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Logger.Log($"{sender ?? "null"}({e})", "closing event sender");
                Stop();
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

            Console.Title = GameConfig.Data.TITLE.Replace("\n", "");
            
            GameConfig.SetupHierarchyFactory();
            Logger.Log(GameConfig.HierarchyFactory.GetType().FullName, "Hierarchy Factory");

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

            SceneManager.StartHierarchy(GameConfig.SceneManager.CurrentHierarchy);

            foreach (var hierarchy in GameConfig.SceneManager.Hierarchies)
            {
                try
                {
                    Logger.Log($"\n{GameConfig.HierarchyFactory.SaveHierarchy(hierarchy)}", "Export Hierarchy");
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
            Logger.Log("Starting Draw Cycle");
            //return;
            if (GameConfig.Data.START_RESIZE_WINDOW)
                Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();

            while (IsWork)
            {
                Input.ConsoleFontUpdate();

                var sleepDelay = (int) (1000.0 / GameConfig.Data.FPS) - (int) watch.ElapsedMilliseconds;
                if (GameConfig.Data.FPS > 0 && sleepDelay > 0)
                    Thread.Sleep(sleepDelay);
                else
                    Thread.Yield();

                watch.Restart();

                if (GameConfig.Data.RESIZE_WINDOW)
                    Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);

                if (!GameConfig.Data.DRAW_PRIOIRITY && MainLoopWorking) continue;

                var matrix = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);

                //Logger.Assert(GameConfig.SceneManager.CurrentHierarchy != null, "GameConfig.GameHierarchy != null");
                //Logger.Assert(GameConfig.RenderFeature != null, "GameConfig.RenderFeature != null");
                DrawLoopWorking = true;

                foreach (IRenderer obj in GameObject.FindAllTypes<IRenderer>(GameConfig.SceneManager.CurrentHierarchy)
                    .OrderBy(c => c.gameObject.layer))
                {
                    try
                    {
                        obj.OnDraw(matrix);
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e, "drawing error");
                    }
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

            watch.Stop();
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

            watch.Stop();
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

            watch.Stop();
        }
    }
}