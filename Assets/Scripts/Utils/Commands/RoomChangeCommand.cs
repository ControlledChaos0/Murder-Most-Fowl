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

    protected override void StopCommand()
    {
        // TO-DO
        // Stopping in the middle seems to be bad
        // See if there's another way to prevent it
        Debug.LogError("Room Change Attempted Stop. This is BAD. Investigate later.");
    }

    protected override void ExecuteCommand()
    {
        RoomTeleport.Teleport();
    }
}
