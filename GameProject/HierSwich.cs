using Engine.Components;
using Engine.Core;
using Engine.Scene;

namespace GameProject
{
    public class HierSwich : Behavior
    {
        public GameObject pix;
        public Camera cam;

        protected override void Start()
        {
            cam = GameObject.FindObjectOfType<Camera>(gameObject.hierarchy);
            Logger.Log(cam);
        }

        protected override void Update()
        {
            var v = Input.ScreenToWorldPosition(Input.GetCursorPosition(), cam);
            pix.transform.LocalPosition = v;

            if (Input.GetKey(Input.Keys.NumPad0))
            {
                GameConfig.SceneManager.SetScene(GameConfig.SceneManager.CurrentHierarchyNumber == 0 ? 1 : 0);
            }
        }
    }
}