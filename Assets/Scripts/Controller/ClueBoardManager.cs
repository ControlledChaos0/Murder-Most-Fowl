using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Clues;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;

public class ClueBoardManager : Singleton<ClueBoardManager>,
    IScrollHandler, IDragHandler, IPointerDownHandler
{
    [Header("Transforms")]
    [SerializeField]
    private Canvas _canvas;
    [SerializeField] 
    private GameObject _board;
    [SerializeField]
    private RectTransform _boardTransform;
    [SerializeField]
    private RectTransform _holdingPinTransform;

    [SerializeField] private RectTransform stringRenderers;
    public RectTransform StringRenderers => stringRenderers;
    
    [SerializeField] private RectTransform clues;
    public RectTransform Clues => clues;

    [Header("Sub-objects")]
    [SerializeField]
    private ClueBoardBin _boardBin;
    [SerializeField]
    private GameObject _toggleButton;
    [SerializeField]
    private GameObject _stickyNote;
    
    [Header("Input")]
    [SerializeField]
    private float _zoomSpeed = 0.05f;

    [SerializeField] private float _zoomOutLimit = 0.328f;
    [SerializeField] private float _zoomInLimit = 1.25f;

    private Animator _animator;
    private ClueObjectUI _selectedObj;

    private Vector2 _boardCenter;
    private Rect _boardBoundsRect;

    private bool _toggleLock;
    private bool _activated;
    private bool _scrollEnabled;

    private readonly Vector2 DEFAULT_PIVOT = new(0.5f, 0.5f);
    private bool _spawnable;

    public Canvas ClueBoardCanvas
    {
        get => _canvas;
    }
    public RectTransform BoardTransform
    {
        get => _boardTransform;
    }
    public RectTransform HoldingPinTransform
    {
        get => _holdingPinTransform;
    }


    private void Awake() {
        InitializeSingleton();
        _toggleLock = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();

        InputController.Instance.ToggleClueBoard += ToggleClueBoard;
        InputController.Instance.ToggleStickyNote += ToggleStickyNote;

        _boardCenter = _boardTransform.parent.position;

        _activated = false;
        _scrollEnabled = true;

        _selectedObj = null;
        _animator = _board.GetComponent<Animator>();

        RectTransform mask = _boardTransform.parent as RectTransform;
        _boardBoundsRect = mask.rect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleClueBoard() {
        if (_toggleLock)
        {
            return;
        }

        if (_activated)
        {
            CloseClueBoard();
        }
        else
        {
            OpenClueBoard();
        }
    }

    private void OpenClueBoard()
    {
        _boardBin.InitBin();
        _activated = true;
        _animator.Play("Reveal");
    }

    private void CloseClueBoard()
    {
        _activated = false;
        _animator.Play("Hide");
    }

    public void LockToggle()
    {
        _toggleLock = true;
        _toggleButton.SetActive(false);
    }

    public void UnlockToggle()
    {
        _toggleLock = false;
        _toggleButton.SetActive(true);
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            return;
        }

        float scroll = eventData.scrollDelta.y;
        float e = 0f;
        if (scroll > 0)
        {
            e = _zoomSpeed;
        }
        else if (scroll < 0)
        {
            e = -_zoomSpeed;
        }

        DynamicZoom(eventData, e);
        ClampBoard();
    }

    private void DynamicZoom(PointerEventData eventData, float zoom)
    {
        Vector2 newCenter = _boardCenter + _boardTransform.anchoredPosition;
        float scale = _boardTransform.localScale.x;
        Vector2 offset = eventData.position - newCenter;
        Vector2 pivot = offset;
        pivot.x *= 1.0f / _boardTransform.sizeDelta.x;
        pivot.y *= 1.0f / _boardTransform.sizeDelta.y;

        // Debug.Log(offset);

        Vector3 tempScale = _boardTransform.localScale + (Vector3.one * zoom);
        if (tempScale.x > _zoomOutLimit && tempScale.x < _zoomInLimit)
        {
            _boardTransform.pivot += pivot;
            _boardTransform.anchoredPosition += (offset * scale);
        }
        if (tempScale.x < _zoomOutLimit)
        {
            tempScale = Vector3.one * _zoomOutLimit;
        }
        if (tempScale.x > _zoomInLimit)
        {
            tempScale = Vector3.one * _zoomInLimit;
        }
        _boardTransform.localScale = tempScale;
    }

    private void ClampBoard()
    {
        float scale = _boardTransform.localScale.x;
        Vector2 pivot = new(_boardTransform.sizeDelta.x * _boardTransform.pivot.x, _boardTransform.sizeDelta.y * _boardTransform.pivot.y);
        Vector2 pivotPos = ((_boardTransform.offsetMin + _boardCenter)) + (pivot);
        Vector2 bottomLeft = (_boardTransform.offsetMin * scale) + (pivotPos - (_boardTransform.anchoredPosition * scale));
        Vector2 topRight = (_boardTransform.offsetMax * scale) + (pivotPos - (_boardTransform.anchoredPosition * scale));
        Debug.DrawLine(bottomLeft, topRight, Color.black);
        Debug.DrawLine(_boardBoundsRect.min + _boardCenter, _boardBoundsRect.max + _boardCenter);

        Vector2 boardMin = _boardBoundsRect.min + _boardCenter;
        Vector2 boardMax = _boardBoundsRect.max + _boardCenter;

        Vector2 newAnchorPos = _boardTransform.anchoredPosition;

        //Y-checking
        if (bottomLeft.y > boardMin.y)
        {
            newAnchorPos.y += (boardMin.y - bottomLeft.y);
        } else if (topRight.y < boardMax.y)
        {
            newAnchorPos.y += (boardMax.y - topRight.y);
        }

        //X-checking
        if (bottomLeft.x > boardMin.x)
        {
            newAnchorPos.x += (boardMin.x - bottomLeft.x);
        }
        else if (topRight.x < boardMax.x)
        {
            newAnchorPos.x += (boardMax.x - topRight.x);
        }

        _boardTransform.anchoredPosition = newAnchorPos;
    }

    public void AddToBin(ClueObjectUI clueObjectUI)
    {
        _boardBin.AddToBin(clueObjectUI);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _boardTransform.anchoredPosition += eventData.delta;
        ClampBoard();
    }

    // Sticky note function
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("TRUEEEEE");
        if (!_spawnable) return;
        var spawnPosition = new Vector3(eventData.position.x, eventData.position.y, 0);
        var parent = ClueBoardManager.Instance.BoardTransform;
        Instantiate(_stickyNote, spawnPosition, Quaternion.identity, parent);
        _spawnable = false;
    }

    public void ToggleStickyNote()
    {
        _spawnable = !_spawnable;
    }
    

}
