using UnityEngine;

public class RoomTeleport : MonoBehaviour
{
    [SerializeField]
    private RoomScreenContainer room;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnMouseDown()
    {
        if (room != null)
        {
            ScreenManager.Instance.ChangeRooms(room);
        }
    }
}
