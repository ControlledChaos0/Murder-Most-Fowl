using Clues;
using UnityEngine;

public class CollectClueCommand : Command
{
    private ClueObject _clueObject;
    public CollectClueCommand(ClueObject clueObject)
    {
        _clueObject = clueObject;
    }

    public override void Stop()
    {
        return;
    }

    protected override void ExecuteCommand()
    {
        _clueObject.CollectClue();
    }

    protected override bool IsCompleted()
    {
        return true;
    }
}
