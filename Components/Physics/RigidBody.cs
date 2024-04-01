using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    public class RigidBody : Component, ICollidable
    {
        public float CollisionOffset = 0.1f;

        public void OnStayColliding(Collision collision)
        {
            transform.LocalPosition += collision.Normal * (collision.Distance + CollisionOffset);
        }
    }
}