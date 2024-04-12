using System.Numerics;
using ConsoleEngine.Components;
using ConsoleEngine.IO;
using ConsoleEngine.Scene;

namespace GameProject
{
    public class Player : Behavior
    {
        private float speed;

        protected override void Update()
        {
            if (Input.GetKey(Input.Keys.Up))
                transform.LocalPosition += new Vector2(0, speed*(float)Time.DeltaTime);
            if (Input.GetKey(Input.Keys.Down))
                transform.LocalPosition += new Vector2(0, -speed*(float)Time.DeltaTime);
            if (Input.GetKey(Input.Keys.Right))
                transform.LocalPosition += new Vector2(speed*(float)Time.DeltaTime, 0);
            if (Input.GetKey(Input.Keys.Left))
                transform.LocalPosition += new Vector2(-speed*(float)Time.DeltaTime, 0);
        }
    }
}