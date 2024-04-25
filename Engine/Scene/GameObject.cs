using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Engine.Components;

namespace Engine.Scene
{
    internal interface IGameObjectUpdatable
    {
        void Update();
    }

    internal interface IGameObjectFixedUpdatable
    {
        void FixedUpdate();
    }

    internal interface IGameObjectStartable
    {
        void Start();
    }

    /// <summary>
    /// World living entity
    /// </summary>
    public class GameObject : IGameObjectUpdatable, IGameObjectStartable, IGameObjectFixedUpdatable
    {
        private List<Component> _components;
        public string name { get; set; }
        public string tag { get; set; }
        public int layer { get; set; }
        public bool active { get; set; }

        /// <summary>
        /// Link to Transform component of GameObject
        /// </summary>
        public Transform transform { get; private set; }

        /// <summary>
        /// Current hierarchy
        /// </summary>
        public Hierarchy hierarchy { get; private set; }

        /// <summary>
        /// Get full hierarchy path
        /// Works on Transform.Parent
        /// </summary>
        public string HierarchyPath => GetHierarchyPath(par: transform);

        private string GetHierarchyPath(string s = "", Transform? par = null)
        {
            if (par?.Parent != null)
            {
                return GetHierarchyPath(par.Parent.gameObject.name + '/' + s, par.Parent);
            }

            return name + '/' + s;
        }

        public GameObject(string name, string tag, int layer, Hierarchy hierarchy)
        {
            _components = new List<Component>();
            this.name = name;
            this.tag = tag;
            this.layer = layer;
            this.hierarchy = hierarchy;

            var tr = new Transform();
            tr.LocalPosition = new Vector2(0, 0);
            tr.Parent = null;
            AddComponent(tr);
            transform = tr;
        }

        void IGameObjectFixedUpdatable.FixedUpdate()
        {
            foreach (var c in _components.Where(c => c.enabled))
            {
                (c as IComponentFixedUpdate)?.FixedUpdate();
            }
        }

        void IGameObjectUpdatable.Update()
        {
            foreach (var c in _components.Where(c => c.enabled))
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

        /// <summary>
        /// Gets component of GameObject
        /// </summary>
        /// <typeparam name="T">Type inherited from Component</typeparam>
        /// <returns></returns>
        public T? GetComponent<T>() where T : Component, IComponentInit =>
            GetAllComponents<T>().FirstOrDefault(defaultValue: null) as T;

        internal Component GetComponentAt(int i) => _components[i];


        /// <summary>
        /// Gets components of type T of GameObject
        /// </summary>
        /// <typeparam name="T">Type inherited from Component</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllComponents<T>() where T : IComponentInit =>
            typeof(T).AssemblyQualifiedName == typeof(Component).AssemblyQualifiedName
                ? _components.Select(c => (T) (c as IComponentInit))
                : _components.Where(ComponentTypeCheck<T>).Select(c => (T) (c as IComponentInit));

        public static bool ComponentTypeCheck<T>(Component c) => ComponentTypeCheck(c, typeof(T));

        public static bool ComponentTypeCheck(Component c, Type type) =>
            c.GetType().AssemblyQualifiedName == type.AssemblyQualifiedName ||
            c.GetType().IsSubclassOf(type) || c.GetType().GetInterfaces().Contains(type);

        /// <summary>
        /// Adds already created Component to GameObject
        /// </summary>
        /// <param name="component">Object of type inherited from Component</param>
        /// <returns>Already created Component</returns>
        public Component AddComponent(Component component)
        {
            if (component is Transform tr)
            {
                transform = tr;
                return transform;
            }

            _components.Add(component);
            (component as IComponentInit).Init(this);
            return component;
        }

        /// <summary>
        /// Adds already created Component to GameObject
        /// </summary>
        /// <param name="component">Object of type inherited from Component</param>
        /// <typeparam name="T">Type inherited from Component</typeparam>
        /// <returns>Already created Component</returns>
        public T? AddComponent<T>(T component) where T : Component
        {
            if (!(component is Component componentInit)) return null;
            _components.Add(componentInit);
            (component as IComponentInit).Init(this);
            return component;
        }

        public void RemoveComponent<T>() where T : IComponentInit => RemoveComponents<T>(1);

        public void RemoveComponents<T>(int count) where T : IComponentInit
        {
            var j = 0;
            for (var i = 0; i < _components.Count; i++)
            {
                if (j == count) return;
                if (ComponentTypeCheck<T>(_components[i])) continue;
                _components.RemoveAt(i);
                j++;
            }
        }

        public void RemoveComponent(Type type) => RemoveComponents(type, 1);

        public void RemoveComponents(Type type, int count)
        {
            var j = 0;
            for (var i = 0; i < _components.Count; i++)
            {
                if (j == count) return;
                if (ComponentTypeCheck(_components[i], type))
                {
                    _components.RemoveAt(i);
                    j++;
                }
            }
        }

        /// <summary>
        /// Finds all objects with tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static IEnumerable<GameObject> FindAllByTag(string tag, Hierarchy hierarchy) =>
            hierarchy.Objects.Where(obj => obj.tag == tag);

        /// <summary>
        /// Finds all objects with name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static IEnumerable<GameObject> FindAllByName(string name, Hierarchy hierarchy) =>
            hierarchy.Objects.Where(obj => obj.name == name);

        /// <summary>
        /// Finds first object with Component
        /// </summary>
        /// <param name="hierarchy">Current hierarchy of object</param>
        /// <typeparam name="T">Type inherited from Component</typeparam>
        /// <returns></returns>
        public static T? FindObjectOfType<T>(Hierarchy hierarchy) where T : Component, IComponentInit =>
            FindAllTypes<T>(hierarchy).First();

        /// <summary>
        /// Finds all objects with Component
        /// </summary>
        /// <param name="hierarchy">Current hierarchy of object</param>
        /// <typeparam name="T">Type inherited from Component</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> FindAllTypes<T>(Hierarchy hierarchy) where T : IComponentInit
        {
            foreach (var gameObject in hierarchy.Objects)
            {
                foreach (var component in gameObject.GetAllComponents<T>())
                {
                    yield return component;
                }
            }
        }

        /// <summary>
        /// Finds first object with tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="hierarchy">Current hierarchy of object</param>
        /// <returns></returns>
        public static GameObject? FindObjectByTag(string tag, Hierarchy hierarchy) =>
            FindAllByTag(tag, hierarchy).FirstOrDefault(defaultValue: null);

        /// <summary>
        /// Finds first object with name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hierarchy">Current hierarchy of object</param>
        /// <returns></returns>
        public static GameObject? FindObjectByName(string name, Hierarchy hierarchy) =>
            FindAllByName(name, hierarchy).FirstOrDefault(defaultValue: null);
    }
}