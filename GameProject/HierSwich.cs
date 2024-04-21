using Engine.Components;
using Engine.Core;
using Engine.Scene;

namespace GameProject
{
    public class HierSwich : Behavior
    {
        public GameObject pix;

        protected override void Update()
        {
            var v = Input.ConsoleToWorldPosition(
                Input.ScreenToConsolePosition(Input.GetCursorPosition()),
                pix.GetComponent<Camera>());
            v = Input.ScreenToConsolePosition(Input.GetCursorPosition());
            //Logger.Log(v);
            //Logger.Log(Input.GetCursorPosition());
            //Logger.Log(Input.GetWindowBounds().Position);
            pix.transform.LocalPosition = v;

            if (Input.GetKey(Input.Keys.NumPad0))
            {
                GameConfig.SceneManager.SetScene(GameConfig.SceneManager.CurrentHierarchyNumber == 0 ? 1 : 0);
            }
        }
    }
}