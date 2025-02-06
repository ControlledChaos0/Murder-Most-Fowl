using UnityEngine;

public class StickyNoteManager : MonoBehaviour
{
    [SerializeField] private GameObject stickyNote;
    
    private Vector3 _spawnPosition;
    private Transform _parent;

    private void Start()
    {
    }
    
    public void createStickyNote()
    {
        _spawnPosition = ClueBoardManager.Instance.BoardTransform.transform.position;
        _parent = ClueBoardManager.Instance.BoardTransform;
        Instantiate(stickyNote, _spawnPosition, Quaternion.identity, _parent);
        Debug.Log("noted!");
    }
}
