using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField]
    private RoomScreenContainer m_CurrentRoomScreenContainer;


    public RoomScreenContainer CurrentRoomScreenContainer {
        get { return m_CurrentRoomScreenContainer;}
        set { m_CurrentRoomScreenContainer = value;}
    }

    private void Awake() {
        InitializeSingleton();
    }
    
    public void ChangeRooms(RoomScreenContainer room, TeleportInfo teleportInfo)
    {
        m_CurrentRoomScreenContainer = room;
        PlayerController.Instance.SwitchCurrentPlayer(room.Player, teleportInfo);
        CameraController.Instance.SwitchCurrentCamera(room.VirtualCamera);
    }

    public Vector2 GetClosestFloorLocation(Ray clickRay) {
        return m_CurrentRoomScreenContainer.Floor.GetClosestFloorLocation(clickRay);
    }

    public Vector2 GetClosestFloorLocation(Vector3 point)
    {
        return m_CurrentRoomScreenContainer.Floor.GetClosestFloorLocation(point);
    }

    public PlayerBody GetCurrentPlayer()
    {
        return m_CurrentRoomScreenContainer.Player;
    }
}
