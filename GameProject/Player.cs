using System.Numerics;
using Engine.Components;
using Engine.Core;
using Engine.Scene;
using Engine.Scene.Serializing;

namespace GameProject
{
    public class Player : Behavior
    {
        private float speed;

        protected override void Start()
        {
            Logger.Log(SerializingHelper.PremadeSerializationFunctions["Vector2"](new Vector2(1, 5.5f)).ToJsonString());
            Logger.Log((Vector2)SerializingHelper.PremadeDeserializationFunctions["Vector2"](SerializingHelper.PremadeSerializationFunctions["Vector2"](new Vector2(1, 5.5f))));
        }

        protected override void Update()
        {
            if (Input.GetKey(Input.Keys.Up))
                transform.LocalPosition += new Vector2(0, speed * (float) Time.DeltaTime);
            if (Input.GetKey(Input.Keys.Down))
                transform.LocalPosition += new Vector2(0, -speed * (float) Time.DeltaTime);
            if (Input.GetKey(Input.Keys.Right))
                transform.LocalPosition += new Vector2(speed * (float) Time.DeltaTime, 0);
            if (Input.GetKey(Input.Keys.Left))
                transform.LocalPosition += new Vector2(-speed * (float) Time.DeltaTime, 0);
        }
    }
}