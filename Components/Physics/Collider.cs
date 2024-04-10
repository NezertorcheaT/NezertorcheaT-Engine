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

        public override string ToString() => $"Collision({Normal}, {Distance})";

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            var t = obj is Collision ? (Collision) obj : (Collision?) null;
            if (t is null) return false;
            return Normal == t.Value.Normal && Distance == t.Value.Distance;
        }

        public static bool operator ==(Collision a, Collision b) => a.Equals(b);
        public static bool operator !=(Collision a, Collision b) => !(a == b);
    }

    public abstract class Collider : Component, IComponentStart
    {
        protected abstract Collision? Check();

        private void IfColliding(Collision collision)
        {
            foreach (var component in gameObject.GetAllComponents<Component>())
            {
                if (component.ActiveAndEnabled && component is ICollidable collidable)
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
                if (!ActiveAndEnabled) continue;
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