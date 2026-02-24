using Manager;
using UnityEngine;

public class MoveCommand : Command
{
    Vector3 _position;
    public MoveCommand(Vector3 position)
    {
        _position = position;
    }

    public override bool IsCompleted()
    {
        return true;
    }
    public override void Stop()
    {
        PlayerController.Instance.StopPlayer();
        CommandManager.Instance.ClearQueue();
    }

    protected override void ExecuteCommand()
    {
        Vector2 pos = ScreenManager.Instance.GetClosestFloorLocation(_position);
        PlayerController.Instance.Move(pos);
    }
}
