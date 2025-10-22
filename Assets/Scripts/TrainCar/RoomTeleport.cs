using System.Drawing;
using Cinemachine;
using UnityEngine;

public class RoomTeleport : MonoBehaviour
{
    [SerializeField]
    private RoomScreenContainer _room;
    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private CinemachineVirtualCamera _newCamera;
    [SerializeField]
    private CinemachineVirtualCamera _currCamera;
    [SerializeField]
    private Transform _door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnMouseDown()
    {
        if (_room != null && _player != null)
        {
            ScreenManager.Instance.ChangeRooms(_room);
            
            _player.Teleport(new Vector3(_door.transform.position.x, _door.transform.position.y - 5, _door.transform.position.z));
            _newCamera.Priority = 10;
            _currCamera.Priority = 1;
        }
    }
}
