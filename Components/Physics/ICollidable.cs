namespace ConsoleEngine.Components.Physics
{
    public interface ICollidable
    {
        void OnStayColliding(Collision collision);
    }
}