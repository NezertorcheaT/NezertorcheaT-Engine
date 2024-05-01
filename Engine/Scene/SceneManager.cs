using System;
using System.Collections.Generic;
using Engine.Core;

namespace Engine.Scene
{
    public class SceneManager
    {
        private Hierarchy[] _hierarchies = new Hierarchy[1];
        public Hierarchy CurrentHierarchy => _hierarchies[CurrentHierarchyNumber];
        public int CurrentHierarchyNumber { get; private set; }
        internal IEnumerable<Hierarchy> Hierarchies => _hierarchies;

        public void SetScene(int sceneNumber)
        {
            CurrentHierarchyNumber = sceneNumber;
            if (!CurrentHierarchy.Started)
                StartHierarchy(CurrentHierarchy);
        }

        internal void InitializeHierarchies(Hierarchy[] hierarchies)
        {
            _hierarchies = hierarchies;
        }


        internal static void StartHierarchy(Hierarchy hierarchy)
        {
            foreach (IGameObjectStartable obj in hierarchy.Objects)
            {
                try
                {
                    obj.Start();
                }
                catch (Exception e)
                {
                    Logger.Log($"{(obj as GameObject)?.name}\n in {hierarchy}{e}", "start error");
                }
            }

            hierarchy.Started = true;
        }
    }
}