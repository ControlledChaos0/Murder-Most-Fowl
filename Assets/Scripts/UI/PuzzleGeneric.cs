using UnityEngine;

public class PuzzleGeneric : MonoBehaviour
{
    [SerializeField] private GameObject puzzleScreenObj;
    
    public void ActivatePuzzleScreen() {
        // actually wait is it instantiate or set active hmmm a real thinker 
        puzzleScreenObj.SetActive(true);
    }

    public void DisablePuzzleScreen() {
        puzzleScreenObj.SetActive(false);
    }
}
