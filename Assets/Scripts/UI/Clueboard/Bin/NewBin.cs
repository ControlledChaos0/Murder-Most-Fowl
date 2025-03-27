using Clues;
using UnityEngine;

public class NewBin : ClueBoardBin
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
    }

    public override void AddToBin(ClueObjectUI clueObject)
    {
        base.AddToBin(clueObject);
        GameManager.StateManager.ActiveState.NewClueBin.Add(clueObject.Clue.ClueID);
    }

    public override void RemoveFromBin(ClueObjectUI clueObject)
    {
        if (!clueObject)
        {
            return;
        }
        base.RemoveFromBin(clueObject);
        GameManager.StateManager.ActiveState.NewClueBin.Remove(clueObject.Clue.ClueID);
    }
}
