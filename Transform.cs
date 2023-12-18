using System.Numerics;

namespace Engine.Components
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
                if (par != null)
                {
                    loc = loc + par.LocalPosition;
                    par = par.Parent;
                    continue;
                }

                return loc;
                break;
            }
        }
    }
}