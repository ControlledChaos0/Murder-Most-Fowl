using System;
using System.Drawing;
using Cinemachine;
using Manager;
using UnityEngine;

public class RoomTeleport : PlayerInteractable
{
    [SerializeField]
    private RoomScreenContainer _currentRoom;
    [SerializeField]
    private TeleportInfo _teleportInfo;

    private TeleportMediator _teleportMediator;
    public TeleportMediator TeleportMediator
    {
        set { _teleportMediator = value; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void OnPointerClick()
    {
        CommandManager.Instance.Queue(new RoomChangeCommand(this));
    }

    public void Teleport()
    {
        _teleportMediator.Teleport(this);
    }

    public void TeleportTo()
    {
        if (_currentRoom != null)
        {
            ScreenManager.Instance.ChangeRooms(_currentRoom, _teleportInfo);
        }
    }
}

[Serializable]
public struct TeleportInfo
{
    [SerializeField] public Transform teleportPos;
    [SerializeField] public PlayerFacing flip;
}
