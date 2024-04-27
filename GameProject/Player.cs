using System.Numerics;
using Engine.Components;
using Engine.Core;
using Engine.Scene;

namespace GameProject
{
    public class Player : Behavior
    {
        public float speed = 1;
        public int walls = 10;
        public int turrets = 1;

        protected override void Update()
        {
            if (Input.GetKey(Input.Keys.Up))
                transform.LocalPosition += new Vector2(0, -speed * (float) Time.DeltaTime);
            if (Input.GetKey(Input.Keys.Down))
                transform.LocalPosition += new Vector2(0, speed * (float) Time.DeltaTime);
            if (Input.GetKey(Input.Keys.Right))
                transform.LocalPosition += new Vector2(-speed * (float) Time.DeltaTime, 0);
            if (Input.GetKey(Input.Keys.Left))
                transform.LocalPosition += new Vector2(speed * (float) Time.DeltaTime, 0);
        }
    }
}