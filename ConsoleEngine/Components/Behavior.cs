using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using ConsoleEngine.Components.Physics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public abstract class Behavior : Component, IComponentStart, IComponentUpdate, ICollidable,IComponentFixedUpdate
    {
        void IComponentStart.Start()
        {
            Start();
        }

        protected virtual void Start()
        {
        }

        void IComponentUpdate.Update()
        {
            if (!ActiveAndEnabled) return;
            Update();
        }
        void IComponentFixedUpdate.FixedUpdate()
        {
            if (!ActiveAndEnabled) return;
            FixedUpdate();
        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
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