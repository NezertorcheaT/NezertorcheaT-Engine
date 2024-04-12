using System.Numerics;
using ConsoleEngine.IO;
using ConsoleEngine.Symbols;

namespace ConsoleEngine.Components
{
    public class Camera : Component
    {
        public Vector2 Offset = new Vector2(
            (int) (GameConfig.Data.WIDTH / 2.0 / Symbol.FiveByEight),
            (int) (GameConfig.Data.HEIGHT / 2.0)
        );
    }
}