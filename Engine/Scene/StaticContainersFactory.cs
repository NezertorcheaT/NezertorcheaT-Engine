﻿using System;
using System.IO;
using System.Linq;
using Engine.Components;
using Engine.Core;

namespace Engine.Scene
{
    public class StaticContainersFactory
    {
        public static void CreateStaticContainers(Hierarchy hierarchy)
        {
            var path = GameConfig.StaticContainersPath;
            var fileString = File.ReadAllText(path);
            var gameObject = new GameObject("_StaticComponentsContainer", "", 0, hierarchy);
            gameObject.active = true;
            
            foreach (var componentLiteral in fileString.Split('\n'))
            {
                var compType = Helper.GetEnumerableOfType<Component>()
                    .FirstOrDefault(component => componentLiteral == component.Name, null);

                if (compType == null) continue;

                var comp = Activator.CreateInstance(compType) as Component;

                gameObject.AddComponent(comp);
                Logger.Log(componentLiteral, "Static Components added");
            }

            hierarchy.Objects.Add(gameObject);
            Logger.Log(gameObject.name, "Static Components Container created");
        }
    }
}