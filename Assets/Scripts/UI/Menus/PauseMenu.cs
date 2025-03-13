using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : Menu
    {
        [SerializeField] private string mainMenuSceneName = "Main Menu";
        public static bool GameIsPaused;

        void Start()
        {
            base.Start();
            InputController.Instance.Pause += OnPause;
            GameIsPaused = false;
        }

        // Update is called once per frame
        private void Update()
        {
        }

        void OnDestroy()
        {
            InputController.Instance.Pause -= OnPause;
        }

        public void OnPause()
        {
            if (GameIsPaused)
            {
                Enter();
            }
            else
            {
                Exit();
            }
        }

        public override void Enter()
        {
            base.Enter();
            Pause();
        }

        public override void Exit()
        {
            base.Exit();
            Resume();
        }

        private void Pause()
        {
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        private void Resume()
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            GameManager.SceneManager.LoadSceneAndSwap(mainMenuSceneName);
        }
        public void QuitGame()
        {
            GameManager.SceneManager.QuitApplication();
        }
    }
}
