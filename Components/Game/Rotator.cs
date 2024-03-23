using ConsoleEngine.IO;

namespace ConsoleEngine.Components.Game
{
    public class Rotator : Behavior
    {
        public override void Update()
        {
            if (Input.GetKey(Input.Keys.NumPad1))
            {
                transform.LocalRotation += 1;
                Logger.Log(transform.Rotation);
            }

            if (Input.GetKey(Input.Keys.NumPad2))
            {
                transform.LocalRotation -= 1;
                Logger.Log(transform.Rotation);
            }
        }
    }
}