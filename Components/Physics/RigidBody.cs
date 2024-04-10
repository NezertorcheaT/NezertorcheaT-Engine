namespace ConsoleEngine.Components.Physics
{
    public class RigidBody : Component, ICollidable
    {
        public float CollisionOffset = 0.05f;

        public void OnStayColliding(Collision collision)
        {
            transform.LocalPosition += collision.Normal * (CollisionOffset);
        }
    }
}