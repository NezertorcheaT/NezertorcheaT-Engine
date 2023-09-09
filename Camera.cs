using System.Numerics;
using Engine.Components;

namespace Engine
{
    public class Camera:Component
    {
        public double[] Offset;
        public double a;
        public bool b;
        public string c;

        public Camera(GameObject gameObject) : base(gameObject)
        {
        }
    }
}