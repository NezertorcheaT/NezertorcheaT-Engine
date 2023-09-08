using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
        public string name { get; set; }
        public string tag { get; set; }
        public int layer { get; set; }
        public Transform transform
        {
            get => GetComponent<Transform>();
        }
        public Hierarchy hierarchy { get; private set; }

        private GameObject(string name, string tag, int layer, Hierarchy hierarchy)
        {
            _components = new List<Component>();
            this.name = name;
            this.tag = tag;
            this.layer = layer;
            this.hierarchy = hierarchy;

            AddComponent<Transform>(new Transform(new Vector2(0, 0), null));
        }
        public static GameObject Instantiate(string name, string tag, int layer, Hierarchy hierarchy)
        {
            var gameObject = new GameObject(name, tag, layer, hierarchy);
            hierarchy.Objs.Add(gameObject);
            ((IGameObjectStartable)gameObject).Start();
            return gameObject;
        }

        void IGameObjectUpdatable.Update()
        {
            foreach (var c in _components)
            {
                (c as IComponentUpdate)?.Update();
            }
        }

        void IGameObjectStartable.Start()
        {
            foreach (var c in _components)
            {
                (c as IComponentStart)?.Start();
            }
        }

        public T GetComponent<T>() where T : Component
        {
            return GetAllComponents<T>()[0] as T;
        }

        public Component[] GetAllComponents<T>() where T : Component
        {
            return (_components.Where(c => c.GetType().FullName == typeof(T).FullName).ToArray());
        }

        public T AddComponent<T>(Component component) where T : Component
        {
            ((IComponentInit) component).Initialise(this);
            _components.Add(component);
            return (T) component;
        }
    }
}