using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Engine.Components;

namespace Engine
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

        public Transform transform { get; private set; }

        public Hierarchy hierarchy { get; private set; }

        public GameObject(string name, string tag, int layer, Hierarchy hierarchy)
        {
            _components = new List<Component>();
            this.name = name;
            this.tag = tag;
            this.layer = layer;
            this.hierarchy = hierarchy;

            var tr = new Transform(this);
            tr.LocalPosition = new Vector2(0, 0);
            tr.Parent = null;
            AddComponent(tr);
            transform = tr;
        }

        public static GameObject Instantiate(string name, string tag, int layer, Hierarchy hierarchy)
        {
            var gameObject = new GameObject(name, tag, layer, hierarchy);
            hierarchy.Objs.Add(gameObject);
            ((IGameObjectStartable) gameObject).Start();
            return gameObject;
        }

        void IGameObjectUpdatable.Update()
        {
            foreach (var c in _components)
            {
                if (!c.enabled) continue;
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

        public T? GetComponent<T>() where T : Component, IComponentInit
        {
            var a = GetAllComponents<T>().ToArray();
            return a.Length == 0 ? null : a[0] as T;
        }

        public IEnumerable<Component>? GetAllComponents<T>() where T : Component, IComponentInit
        {
            return _components.Where(c => c.GetType().FullName == typeof(T).FullName || c.GetType().GetInterfaces().Contains(typeof(T)));
        }

        public Component AddComponent(Component component)
        {
            _components.Add(component);
            ((IComponentInit) component)?.Init(this);
            return component;
        }

        public static IEnumerable<GameObject> FindAllByTag(string tag, Hierarchy hierarchy)
        {
            var r = hierarchy.Objs.Where(obj => obj.tag == tag);
            return r;
        }

        public static T? FindObjectOfType<T>(Hierarchy hierarchy) where T : Component, IComponentInit
        {
            foreach (var obj in hierarchy.Objs)
            {
                foreach (var comp in obj._components)
                {
                    if (comp.GetType().FullName == typeof(T).FullName || comp.GetType().GetInterfaces().Contains(typeof(T)))
                        return comp as T;
                }
            }

            return null;
        }

        public static IEnumerable<Component> FindAllTypes<T>(Hierarchy hierarchy) where T : Component, IComponentInit
        {
            List<Component> l = new List<Component>();
            foreach (var obj in hierarchy.Objs)
            {
                var a = obj.GetAllComponents<T>();
                if(a!=null)
                    l=l.Union(obj.GetAllComponents<T>().ToList()).ToList();
            }

            return l.AsEnumerable();
        }

        public static GameObject? FindObjectByTag(string tag, Hierarchy hierarchy)
        {
            var a = FindAllByTag(tag, hierarchy).ToArray();
            return a.Length == 0 ? null : a[0];
        }
    }
}