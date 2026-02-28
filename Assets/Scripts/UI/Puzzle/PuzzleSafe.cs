using System;
using UI;
using UnityEngine;
using UnityEngine.U2D.IK;

public class PuzzleSafe : Singleton<PuzzleSafe>
{
    [SerializeField]
    private SafeScreen _puzzle4;
    [SerializeField]
    private SafeScreen _puzzle6;

    private Safe _currentSafe;

    public event Action Solve;

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

    public void ActivatePuzzleScreen(Safe safe)
    {
        DisablePuzzleScreen();
        
        _currentSafe = safe;
        switch (safe.Combo.Length)
        {
            case 4: _puzzleActive = _puzzle4; break;
            case 6: _puzzleActive = _puzzle6; break;
            default: Debug.LogError("Shit's fucked. Combo is not 4 or 6"); break;
        }

        _puzzleActive.comboManager.SetCombo(safe.Combo);
        _puzzleActive.comboManager.Solve += _currentSafe.OnSolved;
        // actually wait is it instantiate or set active hmmm a real thinker 
        _puzzleActive.gameObject.SetActive(true);
    }

    public void DisablePuzzleScreen()
    {
        if (_currentSafe != null)
        {
            _puzzleActive.comboManager.Solve -= _currentSafe.OnSolved;
        }
        _currentSafe = null;
        _puzzleActive.gameObject.SetActive(false);
    }
}
