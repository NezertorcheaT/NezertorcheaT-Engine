﻿using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Engine.Components;

namespace Engine
{
    public class Scene
    {
        public static string SaveMap(Hierarchy hierarchy)
        {
            var s = "";
            return s;
        }

        public static Hierarchy GenerateMap(string mapName)
        {
            var path = $"maps\\{mapName}.json";
            var jsonString = File.ReadAllText(path);
            var options = new JsonSerializerOptions {WriteIndented = true};
            var node = JsonNode.Parse(jsonString)!;
            var hierarchy = new Hierarchy();
            Logger.Log(path);
            Logger.Log(options);
            Logger.Log(hierarchy);
            foreach (var objNode in node.AsArray())
            {
                var gameObj = new GameObject(objNode["name"].ToString(), objNode["tag"].ToString(),
                    objNode["layer"].Deserialize<int>(), hierarchy);

                gameObj.transform.LocalPosition = new Vector2(
                    objNode["components"].AsArray().First()["data"].AsArray().First()["value"].AsArray().First()
                        .Deserialize<float>(),
                    objNode["components"].AsArray().First()["data"].AsArray().First()["value"].AsArray().Last()
                        .Deserialize<float>()
                );

                Logger.Log(gameObj);
                foreach (var componentNode in objNode["components"].AsArray())
                {
                    if (componentNode["name"].ToString() == nameof(Transform) ||
                        componentNode["name"].ToString() == nameof(Behavior)) continue;

                    var comp = (Activator.CreateInstance(
                        ReflectiveEnumerator.GetEnumerableOfType<Component>().FirstOrDefault(component =>
                            componentNode["name"].ToString() == component.Name), gameObj) as Component);

                    if (comp == null) continue;
                    Logger.Log(comp);

                    foreach (var varNode in componentNode["data"].AsArray())
                    {
                        Logger.Log(varNode["value"]);
                        foreach (var fieldInfo in comp.GetType().GetFields())
                        {
                            if (fieldInfo.Name != varNode["name"].ToString()) continue;
                            Logger.Log(fieldInfo.FieldType.Name);
                            Logger.Log(fieldInfo.Name);
                            fieldInfo.SetValue(comp, varNode["value"].Deserialize(fieldInfo.FieldType));
                            break;
                        }
                    }

                    gameObj.AddComponent(comp);
                }

                hierarchy.Objs.Add(gameObj);
            }

            return hierarchy;
        }
    }
}