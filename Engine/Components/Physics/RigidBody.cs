namespace Engine.Components.Physics
{
    public class RigidBody : Component, ICollidable
    {
        public bool Static = false;

        public void OnStayColliding(Collision collision)
        {
            if (Static) return;
            transform.LocalPosition += collision.Normal * collision.Distance;
        }
    }
}