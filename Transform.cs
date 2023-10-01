using System.Numerics;

namespace Engine.Components
{
    public class Transform : Component
    {
        public Vector2 LocalPosition;
        public Transform? Parent;
        
        public Vector2 Position => GetPosition(LocalPosition, Parent);

        private Vector2 GetPosition(Vector2 loc, Transform? par) =>
            par != null ? GetPosition(loc + par.LocalPosition, par.Parent) : loc;
    }
}