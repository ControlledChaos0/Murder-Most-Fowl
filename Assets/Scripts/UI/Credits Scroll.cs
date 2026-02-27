using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 40.0f;
    public float endWait = 3.0f;
    public float startY = -841;
    public float endY = 1961;
    private RectTransform scrollWindow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scrollWindow = this.GetComponent<RectTransform>();
        scrollWindow.position = new Vector2(scrollWindow.position.x, scrollWindow.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollWindow.position.y < endY)
        {
            scrollWindow.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        } else
        {
            SceneManager sceneManager = FindFirstObjectByType<SceneManager>();
            sceneManager.LoadSceneAndSwapTransition("Main Menu");
        }
    }

    // public IEnumerator EndCredits()
    // {
    //     yield return new WaitForSeconds(endWait);
    //     SceneManager sceneManager = FindFirstObjectByType<SceneManager>();
    //     sceneManager.LoadSceneAndSwapTransition("Main Menu");
    // }
}
