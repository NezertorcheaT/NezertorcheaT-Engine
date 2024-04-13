using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Nodes;
using Engine.Core;

namespace Engine.Scene.Serializing
{
    public static class SerializingHelper
    {
        public static Dictionary<string, Func<object, JsonNode>> PremadeSerializationFunctions =
            new Dictionary<string, Func<object, JsonNode>>
            {
                {
                    "Vector2", vec =>
                    {
                        var v = (Vector2) vec;
                        return new JsonArray {v.X, v.Y};
                    }
                },
                {
                    "Vector3", vec =>
                    {
                        var v = (Vector3) vec;
                        return new JsonArray {v.X, v.Y, v.Z};
                    }
                },
                {
                    "Vector2Int", vec =>
                    {
                        var v = (Vector2) vec;
                        return new JsonArray {v.X, v.Y};
                    }
                },
                {
                    "Vector3Int", vec =>
                    {
                        var v = (Vector3) vec;
                        return new JsonArray {v.X, v.Y, v.Z};
                    }
                },
                {
                    "Bounds", bounds =>
                    {
                        var b = (Bounds) bounds;
                        return new JsonObject
                        {
                            {"Size", PremadeSerializationFunctions["Vector2"](b.Size)},
                            {"Position", PremadeSerializationFunctions["Vector2"](b.Position)}
                        };
                    }
                },
            };

        public static Dictionary<string, Func<JsonNode, object>> PremadeDeserializationFunctions =
            new Dictionary<string, Func<JsonNode, object>>
            {
                {
                    "Vector2", node => new Vector2(node[0].GetValue<float>(), node[1].GetValue<float>())
                },
                {
                    "Vector3",
                    node => new Vector3(node[0].GetValue<float>(), node[1].GetValue<float>(), node[2].GetValue<float>())
                },
                {
                    "Vector2Int", node => new Vector2(node[0].GetValue<int>(), node[1].GetValue<int>())
                },
                {
                    "Vector3Int",
                    node => new Vector3(node[0].GetValue<int>(), node[1].GetValue<int>(), node[2].GetValue<int>())
                },
                {
                    "Bounds",
                    node => new Bounds(
                        (Vector2) PremadeDeserializationFunctions["Vector2"](node["Size"]),
                        (Vector2) PremadeDeserializationFunctions["Vector2"](node["Position"])
                    )
                },
            };
    }
}