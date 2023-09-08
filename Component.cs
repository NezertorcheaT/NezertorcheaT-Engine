namespace Console_Engine
{
    public interface IComponentInit
    {
        GameObject gameObject { get; }
        void Initialise(GameObject gameObject);
    }

    public interface IComponentUpdate:IComponentInit
    {
        void Update();
    }

    public interface IComponentStart:IComponentInit
    {
        void Start();
    }

    public class Component : IComponentInit
    {
        public GameObject gameObject { get; private set; }

        void IComponentInit.Initialise(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}