using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Game
{
    public class Rotator : Behavior
    {
        private float speed = 0.1f;

        public override void Update()
        {
            if (Input.GetKey(Input.Keys.NumPad1))
            {
                transform.LocalRotation += speed;
                Logger.Log(transform.Rotation);
            }

            if (Input.GetKey(Input.Keys.NumPad2))
            {
                transform.LocalRotation -= speed;
                Logger.Log(transform.Rotation);
            }
        }
    }
}