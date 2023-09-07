using System;
using System.Collections.Generic;
using System.Linq;

namespace Console_Engine
{
    public interface IGameObjectUpdatable
    {
        void Update();
    }

    public interface IGameObjectStartable
    {
        void Start();
    }

    public class GameObject : IGameObjectUpdatable, IGameObjectStartable
    {
        private List<Component> _components;

        void IGameObjectUpdatable.Update()
        {
            foreach (IComponentUpdate c in _components)
            {
                c.Update();
            }
        }

        void IGameObjectStartable.Start()
        {
            foreach (IComponentStart c in _components)
            {
                c.Start();
            }
        }

        public T GetComponent<T>() where T : Component
        {
            return GetAllComponents<T>()[0];
        }

        public T[] GetAllComponents<T>() where T : Component
        {
            return (T[]) (_components.Where(c => c.GetType() == typeof(T)).ToArray());
        }

        public void AddComponent(Component component)
        {
            ((IComponentInit) component).Initialise(this);
            _components.Add(component);
        }
    }
}