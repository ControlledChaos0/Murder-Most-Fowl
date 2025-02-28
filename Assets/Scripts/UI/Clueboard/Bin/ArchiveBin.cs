using Clues;
using UnityEngine;

public class ArchiveBin : ClueBoardBin
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public new void AddToBin(ClueObjectUI clueObject)
    {
        base.AddToBin(clueObject);
        GameManager.StateManager.ActiveState.ArchivedClueBin.Add(clueObject.Clue.ClueID);
    }

    public new void RemoveFromBin(ClueObjectUI clueObject)
    {
        base.RemoveFromBin(clueObject);
        GameManager.StateManager.ActiveState.ArchivedClueBin.Remove(clueObject.Clue.ClueID);
    }
}
