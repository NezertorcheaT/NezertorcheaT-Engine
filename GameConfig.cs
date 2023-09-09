using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Engine
{
    public struct GameConfigData
    {
        public uint HEIGHT{ get; set; }
        public uint WIDTH{ get; set; }
        public string MAP{ get; set; }
        public uint COLLISION_SUBSTEPS{ get; set; }
        public uint RIGIDBODY_SUBSTEPS{ get; set; }
        public uint FPS{ get; set; }
        public double FIXED_REPETITIONS{ get; set; }
        public bool LOG_DRAWCALLS{ get; set; }

        private struct FieldType
        {
            public string Type;
            public string Value;
            public MemberInfo Instance;
        }

        public override string ToString()
        {
            var t = this.GetType();
            var _this = this;

            return $"{t.Name}({t.GetProperties().Select(i => new FieldType {Type = i.PropertyType.Name, Value = i.GetValue(_this) != null ? i.GetValue(_this).ToString() : "null", Instance = i}).ToArray().Aggregate($"this({t.Name}): this", (current, st) => current + $"; {st.Instance.Name}({st.Type}): {st.Value}")})";
        }

    }

    public static class GameConfig
    {
        public static GameConfigData GetData()
        {
            var jsonString = File.ReadAllText("config.json");
            var options = new JsonSerializerOptions { WriteIndented = true };
            JsonNode node = JsonNode.Parse(jsonString)!;
            return JsonSerializer.Deserialize<GameConfigData>(node);
        }
    }
}