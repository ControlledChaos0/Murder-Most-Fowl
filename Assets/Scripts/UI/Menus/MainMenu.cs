using UnityEngine;

public class MainMenu : Menu
{
    public void StartGame()
    {
        GameManager.SceneManager.LoadSceneAndSwapTransition("FinalDialogue");
    }

    public void QuitGame()
    {
        GameManager.SceneManager.QuitApplication();
    }
}
