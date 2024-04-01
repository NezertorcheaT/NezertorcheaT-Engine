using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using ConsoleEngine.Components.Physics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public abstract class Behavior : Component, IComponentStart, IComponentUpdate, ICollidable
    {
        void IComponentStart.Start()
        {
            Start();
            FixedCycle();
        }

        protected virtual void Start()
        {
        }

        void IComponentUpdate.Update()
        {
            if (!ActiveAndEnabled) return;
            Update();
        }

        protected virtual void Update()
        {
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

        public virtual void FixedUpdate()
        {
        }

        void ICollidable.OnStayColliding(Collision collision)
        {
            OnStayCollide(collision);
        }

        protected virtual void OnStayCollide(Collision collision)
        {
        }
    }
}