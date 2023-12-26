using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ConsoleEngine.Components;

namespace ConsoleEngine
{
    public interface IGameObjectUpdatable
    {
        void Update();
    }

    public interface IGameObjectStartable
    {
        void Start();
    }

    /// <summary>
    /// World living entity
    /// </summary>
    public class GameObject : IGameObjectUpdatable, IGameObjectStartable
    {
        private List<Component> _components;
        public string name { get; set; }
        public string tag { get; set; }
        public int layer { get; set; }

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

        private string GetHierarchyPath(string s = "", Transform par = null)
        {
            if (par.Parent != null)
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

        public IEnumerable<Component> GetAllComponents<T>() where T : IComponentInit => _components.Where(
            c => c.GetType().FullName == typeof(T).FullName || c.GetType().GetInterfaces().Contains(typeof(T)));

        /// <summary>
        /// Adds already created Component to GameObject
        /// </summary>
        /// <param name="component">Object of type inherited from Component</param>
        /// <returns></returns>
        public Component AddComponent(Component component)
        {
            _components.Add(component);
            (component as IComponentInit)?.Init(this);
            return component;
        }

        /// <summary>
        /// Finds all objects with tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static IEnumerable<GameObject> FindAllByTag(string tag, Hierarchy hierarchy) =>
            hierarchy.Objs.Where(obj => obj.tag == tag);

        /// <summary>
        /// Finds all objects with name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        public static IEnumerable<GameObject> FindAllByName(string name, Hierarchy hierarchy) =>
            hierarchy.Objs.Where(obj => obj.name == name);

        /// <summary>
        /// Finds first object with Component
        /// </summary>
        /// <param name="hierarchy">Current hierarchy of object</param>
        /// <typeparam name="T">Type inherited from Component</typeparam>
        /// <returns></returns>
        public static T? FindObjectOfType<T>(Hierarchy hierarchy) where T : Component, IComponentInit
        {
            foreach (var obj in hierarchy.Objs)
            {
                foreach (var comp in obj._components)
                {
                    if (comp.GetType().FullName == typeof(T).FullName ||
                        comp.GetType().GetInterfaces().Contains(typeof(T)))
                        return comp as T;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds all objects with Component
        /// </summary>
        /// <param name="hierarchy">Current hierarchy of object</param>
        /// <typeparam name="T">Type inherited from Component</typeparam>
        /// <returns></returns>
        public static IEnumerable<Component> FindAllTypes<T>(Hierarchy hierarchy) where T : IComponentInit =>
            hierarchy.Objs.Aggregate(Array.Empty<Component>() as IEnumerable<Component>,
                (current, obj) => current.Concat(obj.GetAllComponents<T>()));

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