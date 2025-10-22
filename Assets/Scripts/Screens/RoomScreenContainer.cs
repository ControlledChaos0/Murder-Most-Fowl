using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScreenContainer : ScreenContainer
{
    [SerializeField]
    private Floor m_Floor;

    [SerializeField]
    private List<Floor> _floors;

    public Floor Floor {
        get { return m_Floor; }
    }

    public void ChangeFloor(int floorNum)
    {
        if (floorNum > _floors.Count - 1)
        {
            return;
        }
        m_Floor = _floors[floorNum];
    }

    public void Update()
    {
        if (!GameManager.StateManager.ActiveState.Tutorial)
        {
            ChangeFloor(1);
        }
    }
}
