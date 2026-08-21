using Unity.VisualScripting;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    void OnMouseDown()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void OpenTutorial()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
}
