using System;
using System.Collections.Generic;
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
            GameConfig.GetData();
        }

        private static IEnumerable<Hierarchy> SetupHierarchies(string MAP)
        {
            yield return HierarchyFactory.CreateHierarchy(MAP, false);
        }

        public static void BuildMap(string MAP)
        {
            try
            {
                GameConfig.SetupHierarchy(() => SetupHierarchies(MAP));
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

            Logger.Assert(GameConfig.SceneManager.CurrentHierarchy != null, "GameConfig.GameHierarchy != null");
            Logger.Assert(GameConfig.RenderFeature != null, "GameConfig.RenderFeature != null");

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
        }
    }
}