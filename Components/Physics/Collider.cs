using System;
using System.Numerics;
using System.Threading.Tasks;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Physics
{
    public struct Collision
    {
        public Vector2 Normal;
        public float Distance;
    }

    public abstract class Collider : Component, IComponentStart
    {
        protected abstract Collision? Check();

        private void IfColliding(Collision collision)
        {
            foreach (var component in gameObject.GetAllComponents<Component>())
            {
                if (component is ICollidable collidable)
                {
                    collidable.OnStayColliding(collision);
                }
            }
        }

        private void FixedUpdate()
        {
            var collision = Check();
            if (collision is null) return;
            IfColliding(collision.Value);
        }

        void IComponentStart.Start()
        {
            FixedCycle();
        }

        private async void FixedCycle()
        {
            for (;;)
            {
                await Task.Delay((int) (GameConfig.GetData().FIXED_REPETITIONS * 1000));
                if (!enabled) continue;
                try
                {
                    FixedUpdate();
                }
                catch (Exception e)
                {
                    Logger.Log(e, "Fixed update error");
                }
            }
        }
    }
}