using System.Drawing;
using Cinemachine;
using UnityEngine;

public class RoomTeleport : MonoBehaviour
{
    [SerializeField]
    private RoomScreenContainer _room;
    [SerializeField]
    private PlayerController _originalPlayer;
    [SerializeField]
    private PlayerController _newPlayer;
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
        if (_room != null && _newPlayer != null)
        {
            ScreenManager.Instance.ChangeRooms(_room);
            
            _originalPlayer.enabled = false;
            _newPlayer.enabled = true;
            //_player.Teleport(new Vector3(_door.transform.position.x, _door.transform.position.y - 6, _door.transform.position.z));
            _newCamera.Priority = 10;
            _currCamera.Priority = 1;
        }
    }
}
