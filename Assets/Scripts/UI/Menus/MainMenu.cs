using UnityEngine;

public class MainMenu : Menu
{
    public void StartGame()
    {
        GameManager.SceneManager.LoadSceneAndSwap("TrainCarMain");
    }

    public void QuitGame()
    {
        GameManager.SceneManager.QuitApplication();
    }
}
