using UnityEngine;

public class RoomChangeCommand : Command
{
    private RoomTeleport _roomTeleport;
    public RoomChangeCommand(RoomTeleport roomTeleport)
    {
        _roomTeleport = roomTeleport;
    }

    protected override bool IsCompleted()
    {
        return !ScreenManager.Instance.IsChangingRooms;
    }

    public override void Stop()
    {
        // TO-DO
        // Stopping in the middle seems to be bad
        // See if there's another way to prevent it
        throw new System.NotImplementedException();
    }

    protected override void ExecuteCommand()
    {
        _roomTeleport.Teleport();
    }
}
