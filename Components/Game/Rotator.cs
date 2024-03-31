using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Game
{
    public class Rotator : Behavior
    {
        private float speed = 1f;

        public override void FixedUpdate()
        {
            if (Input.GetKey(Input.Keys.NumPad1))
            {
                transform.LocalRotation += speed;
            }

            if (Input.GetKey(Input.Keys.NumPad2))
            {
                transform.LocalRotation -= speed;
            }
        }
    }
}