using System.Numerics;
using Engine.Core;
using Engine.Render.Symbols;

namespace Engine.Components
{
    public class Camera : Component
    {
        public Vector2 Offset = new Vector2(
            (int) (GameConfig.Data.WIDTH / 2.0 / Symbol.FiveByEight),
            (int) (GameConfig.Data.HEIGHT / 2.0)
        );
    }
}