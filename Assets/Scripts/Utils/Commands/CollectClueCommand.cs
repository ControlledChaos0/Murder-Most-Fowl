using Clues;
using UnityEngine;

public class CollectClueCommand : PlayerInteractableCommand
{
    private ClueObject ClueObject {
        get => m_Interactable as ClueObject;
    }

    public CollectClueCommand(ClueObject clueObject) : base(clueObject) { }

    protected override void StopCommand()
    {
        return;
    }

    protected override void ExecuteCommand()
    {
        ClueObject.CollectClue();
    }

    protected override bool IsCompletedInternal()
    {
        return true;
    }
}
