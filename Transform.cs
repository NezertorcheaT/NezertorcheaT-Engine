using System.Numerics;

namespace ConsoleEngine.Components
{
    public class Transform : Component
    {
        public Vector2 LocalPosition;
        public Transform? Parent;

        public Vector2 Position => GetPosition(LocalPosition, Parent);

        private Vector2 GetPosition(Vector2 loc, Transform? par)
        {
            while (true)
            {
                if (par == null) return loc;

                loc += par.LocalPosition;
                par = par.Parent;
            }
        }
    }
}