using Engine.Components;
using Engine.Core;

namespace GameProject
{
    public class HierSwich : Behavior
    {
        protected override void Update()
        {
            if (Input.GetKey(Input.Keys.NumPad0))
            {
                GameConfig.SceneManager.SetScene(GameConfig.SceneManager.CurrentHierarchyNumber == 0 ? 1 : 0);
            }
        }
    }
}