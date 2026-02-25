using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField]
    private RoomScreenContainer m_CurrentRoomScreenContainer;
    [SerializeField]
    private Animator _animator;


    public RoomScreenContainer CurrentRoomScreenContainer {
        get { return m_CurrentRoomScreenContainer; }
        set { m_CurrentRoomScreenContainer = value; }
    }

    private void Awake() {
        InitializeSingleton();
    }

    public void ChangeRooms(RoomScreenContainer room, TeleportInfo teleportInfo)
    {
        StartCoroutine(IChangeRooms(room, teleportInfo));
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

    private IEnumerator IChangeRooms(RoomScreenContainer room, TeleportInfo teleportInfo) {
        _animator.SetTrigger("HideTrigger");
        // Begin Transition
        yield return ITransition("RoomT_Show", "RoomT_ToHide");

        m_CurrentRoomScreenContainer = room;
        PlayerController.Instance.SwitchCurrentPlayer(room.Player, teleportInfo);
        CameraController.Instance.SwitchCurrentCamera(room.VirtualCamera);

        _animator.SetTrigger("ShowTrigger");
    }

    private IEnumerator ITransition(string stateName1, string stateName2)
    {
        bool isTransitioning = true;
        while (isTransitioning)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            int stateHash = Animator.StringToHash(stateName1);
            if (stateInfo.IsName(stateName1) || stateInfo.IsName(stateName2))
            {
                yield return null;
            }
            else
            {
                isTransitioning = false;
            }
        }
    }
}
