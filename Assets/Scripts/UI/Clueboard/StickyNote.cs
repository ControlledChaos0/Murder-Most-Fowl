
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickyNote : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, 
    IPointerDownHandler, IPointerUpHandler, IScrollHandler
{
    [SerializeField] private GameObject stickyNote;
    
    private Vector2 _offset;
    private bool _mouseDown;
    
    private readonly float _sizeMin = .5f;
    private readonly float _sizeMax = 2.5f;
    private readonly Vector3 _scaleChange = new Vector3(.2f, .2f, .2f);
    
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
    
    
    // OnPointerUp/Down makes resizing a bit goofy since its not smooth, potential polish/rework can be made
    public void OnScroll(PointerEventData eventData)
    {
        if (_mouseDown)
        {
            // Increases and decreases size of sprite
            var w = Input.mouseScrollDelta.y;
            Debug.Log(w);
            Debug.Log(_sizeMax > stickyNote.transform.localScale.x);
            if (w > 0 && _sizeMax > stickyNote.transform.localScale.x) {
                stickyNote.transform.localScale += _scaleChange;
            } else if (w < 0 && _sizeMin < stickyNote.transform.localScale.x) {
                stickyNote.transform.localScale -= _scaleChange;
            }
        }
        else
        {
            ClueBoardManager.Instance.OnScroll(eventData);
        }
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
