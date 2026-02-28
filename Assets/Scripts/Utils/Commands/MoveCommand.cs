using Manager;
using UnityEngine;

public class MoveCommand : Command
{
    Vector3 _position;
    public MoveCommand(Vector3 position)
    {
        _position = position;
    }

    protected override bool IsCompletedInternal()
    {
        return !PlayerController.Instance.IsMoving;
    }
    protected override void StopCommand()
    {
        PlayerController.Instance.StopPlayer();
    }

    protected override void ExecuteCommand()
    {
        Vector2 pos = ScreenManager.Instance.GetClosestFloorLocation(_position);
        PlayerController.Instance.Move(pos);
    }

    protected override void ReadyCommand()
    {
        return;
    }
}
