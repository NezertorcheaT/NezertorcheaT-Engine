namespace Engine.Components.Physics
{
    public interface ICollidable
    {
        void OnStayColliding(Collision collision);
    }
}