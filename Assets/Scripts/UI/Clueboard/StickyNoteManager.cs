using UnityEngine;

public class StickyNoteManager : MonoBehaviour
{
    [SerializeField] private GameObject stickyNote;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createStickyNote()
    {
        stickyNote = Instantiate(stickyNote, this.transform);
        Debug.Log("noted!");
    }
}
