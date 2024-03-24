using System.Numerics;

namespace ConsoleEngine.IO
{
    public class Bounds
    {
        public Vector2 Size;
        public Vector2 Position;

        public float Right => Position.X + Size.X / 2f;
        public float Left => Position.X - Size.X / 2f;
        public float Down => Position.Y - Size.Y / 2f;
        public float Up => Position.Y + Size.Y / 2f;

        public Vector2 RightUp => new Vector2(Right, Up);
        public Vector2 LeftUp => new Vector2(Left, Up);
        public Vector2 RightDown => new Vector2(Right, Down);
        public Vector2 LeftDown => new Vector2(Left, Down);

        public Bounds()
        {
            Size = new Vector2();
            Position = new Vector2();
        }

        public Bounds(Vector2 size)
        {
            Size = size;
            Position = new Vector2();
        }

        public Bounds(Vector2 size, Vector2 position)
        {
            Size = size;
            Position = position;
        }
    }
}