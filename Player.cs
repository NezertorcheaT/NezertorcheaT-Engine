using System.Numerics;
using Engine.Components;

namespace Engine
{
    public class Player : Behavior
    {
        public float speed;

        public override void Start()
        {
            foreach (var v in GameObject.FindAllTypes<Transform>(gameObject.hierarchy))
            {
                Logger.Log(v, "ass");
            }
        }

        public override void Update()
        {
            if (Input.GetKey(Input.Keys.Up))
                transform.LocalPosition += new Vector2(0, speed);
            if (Input.GetKey(Input.Keys.Down))
                transform.LocalPosition += new Vector2(0, -speed);
            if (Input.GetKey(Input.Keys.Right))
                transform.LocalPosition += new Vector2(speed, 0);
            if (Input.GetKey(Input.Keys.Left))
                transform.LocalPosition += new Vector2(-speed, 0);
        }
    }
}