namespace Console_Engine
{
    public interface IComponentInit
    {
        GameObject gameObject { get; }
        void Initialise(GameObject gameObject);
    }

    public interface IComponentUpdate
    {
        void Update();
    }

    public interface IComponentStart
    {
        void Start();
    }

    public class Component : IComponentInit
    {
        public GameObject gameObject { get; private set; }

        public Component()
        {
        }

        void IComponentInit.Initialise(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}