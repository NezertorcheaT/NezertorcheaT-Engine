using System.Numerics;

namespace Engine.Components
{
    public class Transform : Component
    {
        public Vector2 LocalPosition;
        public Transform Parent;
        public Vector2 Position => GetPosition(LocalPosition, Parent);

        private Vector2 GetPosition(Vector2 loc, Transform par)
        {
            if (par != null)
            {
                return GetPosition(loc + par.LocalPosition, par.Parent);
            }
            else
            {
                return loc;
            }
        }

        public Transform(GameObject gameObject) : base(gameObject)
        {
        }
    }
}