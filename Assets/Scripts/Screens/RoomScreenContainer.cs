using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScreenContainer : ScreenContainer
{
    [SerializeField]
    private PlayerBody m_Player;
    [SerializeField]
    private Floor m_Floor;

    [SerializeField]
    private List<Floor> _floors;

    public Floor Floor {
        get { return m_Floor; }
    }
    public PlayerBody Player
    {
        get { return m_Player; }
    }

    public void ChangeFloor(int floorNum)
    {
        if (floorNum > _floors.Count - 1)
        {
            return;
        }
        m_Floor = _floors[floorNum];
    }

    protected override void Update()
    {
        base.Update();
        if (!GameManager.StateManager.ActiveState.Tutorial)
        {
            ChangeFloor(1);
        }
    }
}
