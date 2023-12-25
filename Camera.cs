using Engine.Components;

namespace Engine
{
    public class Camera : Component
    {
        public double[] Offset = {GameConfig.Data.WIDTH/2.0/Symbol.FiveByEight, GameConfig.Data.HEIGHT/2.0};
        public double a;
        public bool b;
        public string c;
    }
}