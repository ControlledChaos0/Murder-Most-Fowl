using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClueObjectUI : MonoBehaviour, 
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IScrollHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private static readonly float _sizeMin = .25f;
    [SerializeField]
    private static readonly float _sizeMax = 1.5f;

    private Vector2 _offset;
    private bool _mouseDown;

    

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = ClueBoardManager.Instance.HoldingPinTransform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        ClueBoardManager.Instance.AddToBin(this);
        _mouseDown = false;
    }

    void StartDelay()
    {
        ClueBoardManager.Instance.AddToBin(this);
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

    public void OnScroll(PointerEventData eventData)
    {
        // TODO
        // Get resizing working for clueobjects
        if (_mouseDown)
        {
            Debug.Log("Yippee!");
        }
        else
        {
            ClueBoardManager.Instance.OnScroll(eventData);
        }
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
