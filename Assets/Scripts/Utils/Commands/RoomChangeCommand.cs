using UnityEngine;

public class RoomChangeCommand : Command
{
    private RoomTeleport _roomTeleport;
    public RoomChangeCommand(RoomTeleport roomTeleport)
    {
        _roomTeleport = roomTeleport;
    }

    public override bool IsCompleted()
    {
        return true;
    }

    public override void Stop()
    {
        throw new System.NotImplementedException();
    }

    protected override void ExecuteCommand()
    {
        _roomTeleport.Teleport();
    }
}
