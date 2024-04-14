using System;
using Engine.Components;
using Engine.Core;
using Engine.Render.Symbols;
using Engine.Scene;

namespace EditorPreview
{
    public static class Preview
    {
        public static void Start()
        {
            Logger.Initialise();
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Logger.Log(sender ?? "null", "closing event sender");
                Logger.Stop();
            };

            try
            {
                GameConfig.GetData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "game config error");
            }
        }

        public static void BuildMap()
        {
            try
            {
                GameConfig.SetupHierarchy(() => HierarchyFactory.CreateHierarchy(GameConfig.Data.MAP, true));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
            }
        }

        public static void ShowMap()
        {
            GameConfig.SetupRenderFeature();
            Logger.Log(GameConfig.RenderFeature.GetType().FullName, "render feature");


            if (GameConfig.Data.START_RESIZE_WINDOW)
                Console.SetWindowSize((int) GameConfig.Data.WIDTH + 2, (int) GameConfig.Data.HEIGHT + 2);

            var matrix = new SymbolMatrix(GameConfig.Data.WIDTH, GameConfig.Data.HEIGHT);

            Logger.Assert(GameConfig.GameHierarchy != null, "GameConfig.GameHierarchy != null");
            Logger.Assert(GameConfig.RenderFeature != null, "GameConfig.RenderFeature != null");

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

            try
            {
                GameConfig.RenderFeature.RenderProcedure(matrix);
            }
            catch (Exception e)
            {
                Logger.Log(e, "drawing error");
            }
        }
    }
}