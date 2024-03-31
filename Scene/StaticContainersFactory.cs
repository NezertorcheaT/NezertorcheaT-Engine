using System;
using System.IO;
using System.Linq;
using ConsoleEngine.Components;
using ConsoleEngine.IO;

namespace ConsoleEngine.Scene
{
    public class StaticContainersFactory
    {
        public static void CreateStaticContainers(Hierarchy hierarchy)
        {
            var path = GameConfig.StaticContainersPath;
            var fileString = File.ReadAllText(path);
            var gameObject = new GameObject("_StaticComponentsContainer", "", 0, hierarchy);
            
            foreach (var componentLiteral in fileString.Split('\n'))
            {
                var compType = Helper.GetEnumerableOfType<Component>()
                    .FirstOrDefault(component => componentLiteral == component.Name, null);

                if (compType == null) continue;

                var comp = Activator.CreateInstance(compType) as Component;

                gameObject.AddComponent(comp);
            }

            hierarchy.Objects.Add(gameObject);
        }
    }
}