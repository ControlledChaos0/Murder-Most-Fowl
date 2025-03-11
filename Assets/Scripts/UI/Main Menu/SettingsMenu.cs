using GlobalManagers;
using UnityEngine;
public class SettingsMenu : MonoBehaviour
{
    public void settings()
    {
        GameManager.SceneManager.LoadSceneAndSwap("Settings Menu");
    }
}
