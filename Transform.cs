using System.Numerics;

namespace Console_Engine
{
    public class Transform : Component
    {
        public Transform(Vector2 localPosition, Transform parent)
        {
            LocalPosition = localPosition;
            Parent = parent;
        }

        public Vector2 LocalPosition { get; set; }
        public Transform Parent { get; private set; }
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

    }
}