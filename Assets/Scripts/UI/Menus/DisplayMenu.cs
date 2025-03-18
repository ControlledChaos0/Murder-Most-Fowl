using UnityEngine;

public class DisplayMenu : MonoBehaviour
{
    private Resolution[] _resolutions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _resolutions = Screen.resolutions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
