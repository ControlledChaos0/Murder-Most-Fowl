using GlobalManagers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName = "Main Menu";
        public static bool GameIsPaused;
        
        public GameObject pauseMenuUI;

        void Start()
        {
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
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        private void Resume()
        {
            pauseMenuUI.SetActive(false);
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
