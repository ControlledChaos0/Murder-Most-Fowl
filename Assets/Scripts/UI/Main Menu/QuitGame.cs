using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void doExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
