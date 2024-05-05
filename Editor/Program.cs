using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;
using Engine.Components;
using Engine.Core;
using Engine.Scene;

namespace Editor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logger.Initialise();
            GameConfig.GetData();

            static IEnumerable<Hierarchy> SetupHierarchies()
            {
                foreach (var map in GameConfig.Data.MAPS)
                {
                    yield return GameConfig.HierarchyFactory.CreateHierarchy(File.ReadAllText(map),
                        Path.GetFileName(map), false);
                }
            }

            Input.ConsoleFontUpdate();

            try
            {
                GameConfig.SetupHierarchy(SetupHierarchies);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, "scene build error");
            }

            Process.Start(new ProcessStartInfo("EditorPreview.exe")
                {
                    Arguments = GameConfig.Data.MAPS[0],
                    UseShellExecute = true
                }
            );

            /*var iv = new InspectorView();
            iv.Wight = 100;
            iv.Height = 100;
            foreach (var gameObject in GameConfig.GameHierarchy.Objects)
            {
                iv.GameObject = gameObject;
                Logger.Log(iv.Draw(), "drawing");
            }*/
            var pipp = "iv.Draw()\npipidastr";
            Logger.Log(pipp.Wight(), "Wight");
            Logger.Log(pipp.Lines(), "Lines");
            Logger.Log(pipp.At(new Vector2(4, 1)), "drawing");
            Logger.Log(pipp.PosToInd(new Vector2(4, 1)), "drawing");
            Logger.Log('\n' + pipp.Insert("penis", new Vector2(5, 2)), "drawing");
            Logger.Log('\n' + pipp.Offset(new Vector2(5, 2)), "drawing");

            Console.ReadKey();
            Logger.Stop();
        }
    }

    public abstract class Widget
    {
        public uint Wight { get; set; }
        public uint Height { get; set; }
        public Widget[] Siblings;

        public static string TabulationLiteral => "  ";
        public static string DoublePointsLiteral => ": ";
        public static string NextLineLiteral => "\n";

        public abstract string Draw();
    }

    public class InspectorView : Widget, IScrollable
    {
        public GameObject GameObject;
        private int _scrollOffset;

        public override string Draw()
        {
            var sb = new StringBuilder();

            foreach (var component in GameObject.GetAllComponents<Component>())
            {
                var componentType = component.GetType();

                if (sb.Lines() >= Height - 1) return sb.ToString();
                sb.Append(componentType.Name);

                foreach (var fieldInfo in component.GetType().GetFields())
                {
                    if (sb.Lines() >= Height - 1) return sb.ToString();
                    sb.Append(NextLineLiteral);
                    sb.Append(TabulationLiteral);
                    sb.Append(fieldInfo.Name);
                    sb.Append(DoublePointsLiteral);
                    sb.Append(fieldInfo.GetValue(component));
                }

                if (sb.Lines() >= Height - 1) return sb.ToString();
                sb.Append(NextLineLiteral);
            }

            return sb.ToString();
        }

        void IScrollable.ScrollUp(int n) => _scrollOffset -= n;
        void IScrollable.ScrollDown(int n) => _scrollOffset += n;
    }

    public class HierarchyView : Widget, IScrollable
    {
        private Hierarchy Hierarchy => GameConfig.SceneManager.CurrentHierarchy!;
        private int _scrollOffset;

        public override string Draw()
        {
            return "";
        }

        void IScrollable.ScrollUp(int n) => _scrollOffset -= n;
        void IScrollable.ScrollDown(int n) => _scrollOffset += n;
    }

    public abstract class Layout : Widget
    {
    }

    public class HorizontalLayout : Layout
    {
        public override string Draw()
        {
            var w = (uint) (Wight / Siblings.Length);
            foreach (var child in Siblings)
            {
                child.Wight = w;
            }

            var str = "";
            for (var i = 0; i < Siblings.Length; i++)
            {
                str = str.Insert(Siblings[i].Draw(), new Vector2(0, i * w));
            }

            return str;
        }
    }

    public interface IScrollable
    {
        void ScrollUp(int n);
        void ScrollDown(int n);
    }

    public interface ISelectableScrollable : IScrollable
    {
        int Selection { get; set; }
    }

    public interface IMultiSelectableScrollable : IScrollable
    {
        List<int> Selections { get; set; }
    }
}