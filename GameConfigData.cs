using System.Reflection;
using System.Text;

namespace Engine
{
    interface IJsonable
    {
        string Json { get; }
    }

    /// <summary>
    /// Game settings data
    /// </summary>
    public struct GameConfigData : IJsonable
    {
        public uint HEIGHT { get; set; }
        public uint WIDTH { get; set; }
        public string MAP { get; set; }
        public uint COLLISION_SUBSTEPS { get; set; }
        public uint RIGIDBODY_SUBSTEPS { get; set; }
        public uint DRAW_BUFFER_SIZE { get; set; }
        public uint FPS { get; set; }
        public double FIXED_REPETITIONS { get; set; }
        public float CONSOLE_LINE_RENDERER_DELAY { get; set; }
        public bool LOG_DRAWCALLS { get; set; }

        private static readonly string AsString = "NezertorcheatIsGandonAndUebok";

        private struct FieldType
        {
            public string Type;
            public string Value;
            public MemberInfo Instance;
        }

        public override string ToString()
        {
            var t = GetType();
            var s = new StringBuilder($"{t.Name}(");

            s.Append(AsString);
            foreach (var i in t.GetProperties())
            {
                var f = new FieldType
                {
                    Type = i.PropertyType.Name,
                    Value = i.GetValue(this) != null ? i.GetValue(this).ToString() : "null",
                    Instance = i
                };
                s.Append($"; {f.Instance.Name}({f.Type}): {f.Value}");
            }

            s.Append(')');

            return s.ToString().Replace($"{AsString}; ", "");
        }

        string IJsonable.Json => ToJson();

        private string ToJson()
        {
            var t = GetType();
            var s = new StringBuilder("{");
            s.Append(AsString);

            foreach (var i in t.GetProperties())
            {
                var f = new FieldType
                {
                    Type = i.PropertyType.Name,
                    Value = i.GetValue(this) != null ? i.GetValue(this).ToString() : "null",
                    Instance = i
                };
                s.Append($", \"{f.Instance.Name}\": {(f.Type == "String" ? $"\"{f.Value}\"" : f.Value)}");
            }

            s.Append('}');

            var str = s.ToString().Replace($"{AsString}, ", "");
            Logger.Log(str, "config json");
            return str;
        }
    }
}