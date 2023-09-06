using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Console_Engine
{
    public struct GameConfigData
    {
        public int HEIGHT{ get; set; }
        public int WIDTH{ get; set; }
        public string MAP{ get; set; }
        public int COLLISION_SUBSTEPS{ get; set; }
        public int RIGIDBODY_SUBSTEPS{ get; set; }
        public int FPS{ get; set; }
        public float FIXED_REPETITIONS{ get; set; }
        public bool LOG_DRAWCALLS{ get; set; }

        private struct FieldType
        {
            public string type;
            public string value;
            public MemberInfo i;
        }

        public override string ToString()
        {
            var t = this.GetType();
            var _this = this;

            FieldType[] ft = t.GetProperties().Select(i => new FieldType
            {
                type = i.PropertyType.Name,
                value = i.GetValue(_this) != null ? i.GetValue(_this).ToString() : "null",
                i=i
            }).ToArray();


            return $"{t.Name}({FieldTypeArrayToString(ft)})";
        }

        private string FieldTypeArrayToString(FieldType[] ss)
        {
            return ss.Aggregate($"this: this", (current, st) => current + $"; {st.i.Name}({st.type}): {st.value}");
        }
    }

    public static class GameConfig
    {
        public static GameConfigData GetData()
        {
            return JsonSerializer.Deserialize<GameConfigData>(File.ReadAllText("config.json"));
        }
    }
}