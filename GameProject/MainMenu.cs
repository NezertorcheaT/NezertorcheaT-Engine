using Engine.Components;
using Engine.Core;

namespace GameProject
{
    public class MainMenu : Behavior
    {
        public Button enter;
        public Button exit;

        protected override void Start()
        {
            enter.OnClick += Enter;
            exit.OnClick += Exit;
        }

        private void Enter()
        {
            enter.enabled = false;
            exit.enabled = false;
            Logger.Log("Enter");
            Startup.Stop();
            exit.OnClick -= Exit;
            enter.OnClick -= Enter;
        }
        private void Exit()
        {
            enter.enabled = false;
            exit.enabled = false;
            Logger.Log("Exit");
            GameConfig.SceneManager.SetScene(1);
            enter.OnClick -= Enter;
            exit.OnClick -= Exit;
        }
    }
}