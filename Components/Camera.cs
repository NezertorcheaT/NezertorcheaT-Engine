using ConsoleEngine.IO;
using ConsoleEngine.Symbols;

namespace ConsoleEngine.Components
{
    public class Camera : Component
    {
        public double[] Offset =
        {
            (int) (GameConfig.Data.WIDTH / 2.0 / Symbol.FiveByEight),
            (int) (GameConfig.Data.HEIGHT / 2.0)
        };

        public double a;
        public bool b;
        public string c;
    }
}