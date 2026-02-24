using System;
using UnityEngine;

public class TeleportManager : Singleton<TeleportManager>
{
    [SerializeField]
    private TeleportMediator[] teleports;

    private void Awake()
    {
        InitializeSingleton();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var teleport in teleports)
        {
            teleport.SetMediator();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class TeleportMediator
{
    [SerializeField] private RoomTeleport door1;
    [SerializeField] private RoomTeleport door2;

    public void SetMediator()
    {
        door1.TeleportMediator = this;
        door2.TeleportMediator = this;
    }

    public void Teleport(RoomTeleport door)
    {
        if (door == door1)
        {
            door2.TeleportTo();
        }
        else if (door == door2)
        {
            door1.TeleportTo();
        }
        else
        {
            Debug.LogError("Teleport failed due to doors not valid");
        }
    }
}
