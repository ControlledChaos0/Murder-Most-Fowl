
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickyNote : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, 
    IPointerDownHandler, IPointerUpHandler, IScrollHandler
{
    [SerializeField] private GameObject stickyNote;
    [SerializeField] private TMP_InputField inputField;
    
    private Vector2 _offset;
    private bool _mouseDown;
    private bool _new;
    private bool _isFocused;
    
    private readonly float _sizeMin = .5f;
    private readonly float _sizeMax = 2.5f;
    private readonly Vector3 _scaleChange = new Vector3(.2f, .2f, .2f);

    void Start()
    {
        _new = true;
        _isFocused = false;
    }
    void Update()
    {
        Debug.Log($"IsFocus is {inputField.isFocused}");
        Debug.Log($"ToggleLock is {ClueBoardManager.Instance.ToggleLock}");

        if (inputField.isFocused != _isFocused)
        {
            ClueBoardManager.Instance.ToggleLock = inputField.isFocused;
        }

        _isFocused = inputField.isFocused;
    }

    void OnDestroy()
    {
        ClueBoardManager.Instance.ToggleLock = false;
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

        if (_new)
        {
            _offset = Vector2.zero;
        }

        transform.parent = ClueBoardManager.Instance.Front;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _offset = Vector2.zero;
        List<RaycastResult> results = new List<RaycastResult>();
        ClueBoardManager.Instance.GraphicRaycast.Raycast(eventData, results);
        bool placed = false;
        foreach (RaycastResult result in results)
        {
            if (placed)
            {
                break;
            }
            switch (result.gameObject.tag)
            {
                case ("Board"):
                    transform.parent = ClueBoardManager.Instance.Clues;
                    placed = true;
                    break;
            }
        }
        _offset = Vector2.zero;

        if (!placed)
        {
            Destroy(gameObject);
        }

        _new = false;
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
            ClueBoard.Instance.OnScroll(eventData);
        }
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
