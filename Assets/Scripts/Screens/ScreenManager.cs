using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField]
    private RoomScreenContainer m_CurrentRoomScreenContainer;


    public RoomScreenContainer CurrentRoomScreenContainer {
        get { return m_CurrentRoomScreenContainer;}
        private set { m_CurrentRoomScreenContainer = value;}
    }

    private void Awake() {
        InitializeSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetClosestFloorLocation(Ray clickRay) {
        return m_CurrentRoomScreenContainer.Floor.GetClosestFloorLocation(clickRay);
    }

    public Vector2 GetClosestFloorLocation(Vector3 point)
    {
        return m_CurrentRoomScreenContainer.Floor.GetClosestFloorLocation(point);
    }
}
