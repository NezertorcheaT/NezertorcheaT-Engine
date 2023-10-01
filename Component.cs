namespace Engine.Components
{
    public interface IComponentInit
    {
        GameObject gameObject { get; }
        void Init(GameObject gameObject);
    }

    public interface IComponentUpdate : IComponentInit
    {
        void Update();
    }

    public interface IComponentStart : IComponentInit
    {
        void Start();
    }

    public abstract class Component : IComponentInit
    {
        public GameObject gameObject { get; private set; }
        public Transform transform => gameObject.transform;

        public bool enabled = true;


        void IComponentInit.Init(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}