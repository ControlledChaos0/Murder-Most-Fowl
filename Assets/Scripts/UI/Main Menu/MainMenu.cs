using UnityEngine;
public class ReturnMain : MonoBehaviour
{
    public void settings()
    {
        GameManager.SceneManager.LoadSceneAndSwap("Main Menu");
    }
}
