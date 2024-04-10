using System.Numerics;
using ConsoleEngine.Components.Physics;
using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Game
{
    public class Player2 : Behavior
    {
        private float speed;

        protected override void Update()
        {
            if (Input.GetKey(Input.Keys.NumPad8))
                transform.LocalPosition += new Vector2(0, speed*(float)Time.DeltaTime);
            if (Input.GetKey(Input.Keys.NumPad2))
                transform.LocalPosition += new Vector2(0, -speed*(float)Time.DeltaTime);
            if (Input.GetKey(Input.Keys.NumPad6))
                transform.LocalPosition += new Vector2(speed*(float)Time.DeltaTime, 0);
            if (Input.GetKey(Input.Keys.NumPad4))
                transform.LocalPosition += new Vector2(-speed*(float)Time.DeltaTime, 0);
        }

        protected override void OnStayCollide(Collision collision)
        {
            Logger.Log("ALERT ALERT COLLISION DETECTED IN U LOCATION");
        }
    }
}