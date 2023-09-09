using System;
using System.Numerics;
using System.Threading.Tasks;
using Engine.Components;

namespace Engine
{
    internal static class Program
    {
        public static async void DrawCycle(Hierarchy h)
        {
            var d = GameConfig.GetData();
            SymbolMatrix m;
            
            while (true)
            {
                await Task.Delay((int) (1.0 / (double) d.FPS * 1000.0));
                
                Console.SetWindowSize((int) d.WIDTH + 2, (int) d.HEIGHT + 2);

                m = new SymbolMatrix(d.WIDTH, d.HEIGHT);

                foreach (IRenderer obj in GameObject.FindAllTypes<Drawer>(h))
                {
                    obj.DrawSymbol(m, (obj as Component).transform.Position,
                        new Symbol {Character = 'O', Color = ConsoleColor.White});
                }

                Console.Clear();
                //Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Console.Write(m.ToString());
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
                var d = GameConfig.GetData();
                var m = new SymbolMatrix(d.WIDTH, d.HEIGHT);
                var h = Scene.GenerateMap(d.MAP);
                Logger.Log(d.ToString());

                foreach (IGameObjectStartable obj in h.Objs)
                {
                    obj.Start();
                }

                DrawCycle(h);
                while (true)
                {
                    foreach (IGameObjectUpdatable obj in h.Objs)
                    {
                        obj.Update();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Log(e, e.GetType().Name);
            }

            Logger.Stop();
            Console.ReadKey();
        }
    }
}