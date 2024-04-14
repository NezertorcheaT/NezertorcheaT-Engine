using System;
using System.Diagnostics;
using Engine.Core;
using Engine.Scene;

namespace Editor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            GameConfig.GetData();

            try
            {
                GameConfig.SetupHierarchy(() => HierarchyFactory.CreateHierarchy(GameConfig.Data.MAP, true));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
            }

            Process.Start(new ProcessStartInfo("EditorPreview.exe")
                {
                    Arguments = GameConfig.Data.MAP,
                    UseShellExecute = true
                }
            );

            Console.ReadKey();
        }
    }
    
    public abstract class Widget
    {
        public uint Wight { get; set; }
        public uint Height { get; set; }
        public Widget[] Siblings;

        public abstract string Draw();
    }

    public class InspectorView : Widget
    {
        public GameObject GameObject;

        public InspectorView(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public override string Draw()
        {
            return "";
        }
    }

    public class HierarchyView : Widget
    {
        private Hierarchy Hierarchy => GameConfig.GameHierarchy!;
        
        public override string Draw()
        {
            return "";
        }
    }

    public abstract class Layout : Widget
    {
    }

    public abstract class HorizontalLayout : Widget
    {
    }

    public interface IScrollable
    {
        
    }
}