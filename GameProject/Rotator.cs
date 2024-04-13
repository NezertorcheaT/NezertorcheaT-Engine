using Engine.Components;
using Engine.Core;

namespace GameProject
{
    public class Rotator : Behavior
    {
        private float speed = 1f;

        protected override void FixedUpdate()
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