using UnityEngine;
using UnityEngine.EventSystems;

public class StickyNoteSpawner : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject stickyNotePrefab;

    private StickyNote _dragNote;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject newStickyNote = Instantiate(stickyNotePrefab);

        newStickyNote.gameObject.transform.position = eventData.position;
        newStickyNote.gameObject.transform.localScale = Vector3.one;
        eventData.pressPosition = eventData.position;

        _dragNote = newStickyNote.GetComponent<StickyNote>();
        _dragNote.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragNote.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragNote.OnEndDrag(eventData);
        _dragNote = null;
    }
}
