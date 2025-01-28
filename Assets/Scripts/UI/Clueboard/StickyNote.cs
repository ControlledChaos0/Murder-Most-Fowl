
using UnityEngine;
using UnityEngine.EventSystems;

public class StickyNote : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, 
    IPointerDownHandler, IPointerUpHandler
{
    private Vector2 _offset;
    private bool _mouseDown;
    private GameObject _image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.pressPosition;
        Vector2 uiPos = transform.position;
        _offset = uiPos - mousePos;

        transform.parent = ClueBoardManager.Instance.BoardTransform;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _offset = Vector2.zero;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _mouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _mouseDown = false;
    }
}
