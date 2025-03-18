using GlobalManagers;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void startGame()
    {
        GameManager.SceneManager.LoadSceneAndSwap("TrainCarMain");
    }
}