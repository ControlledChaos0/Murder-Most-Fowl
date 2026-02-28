using UI;
using UnityEngine;

public class PuzzleSafe : Singleton<PuzzleSafe>
{
    [SerializeField]
    private SafeScreen _puzzle4;
    [SerializeField]
    private SafeScreen _puzzle6;

    public enum Code {
        FOUR,
        SIX
    }

    private SafeScreen _puzzleActive;
    void Awake()
    {
        InitializeSingleton();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _puzzleActive = _puzzle4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePuzzleScreen(Code code)
    {
        DisablePuzzleScreen();
        switch (code)
        {
            case Code.FOUR: _puzzleActive = _puzzle4; break;
            case Code.SIX: _puzzleActive = _puzzle6; break;
        }
        // actually wait is it instantiate or set active hmmm a real thinker 
        _puzzleActive.gameObject.SetActive(true);
    }

    public void DisablePuzzleScreen()
    {
        _puzzleActive.gameObject.SetActive(false);
    }
}
