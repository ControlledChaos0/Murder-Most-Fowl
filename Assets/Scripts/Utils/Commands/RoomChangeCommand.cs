using UnityEngine;

public class RoomChangeCommand : PlayerInteractableCommand
{
    private RoomTeleport RoomTeleport
    {
        get => m_Interactable as RoomTeleport;
    }
    public RoomChangeCommand(RoomTeleport roomTeleport) : base(roomTeleport) { }

    protected override bool IsCompletedInternal()
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
        RoomTeleport.Teleport();
    }
}
