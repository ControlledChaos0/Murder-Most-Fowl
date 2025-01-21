using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName = "Main Menu";
        public static bool GameIsPaused;
        
        public GameObject pauseMenuUI;

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
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

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
