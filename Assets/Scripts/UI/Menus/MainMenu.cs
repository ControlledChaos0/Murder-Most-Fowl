using UnityEngine;

public class MainMenu : Menu
{
    public void StartGame()
    {
        GameManager.SceneManager.LoadSceneAndSwap("FinalDialogue");
    }

    public void QuitGame()
    {
        GameManager.SceneManager.QuitApplication();
    }
}
