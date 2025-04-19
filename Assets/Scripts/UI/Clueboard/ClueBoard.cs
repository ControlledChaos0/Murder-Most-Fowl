using UnityEngine;
using UnityEngine.EventSystems;

public class ClueBoard : Singleton<ClueBoard>,
    IDragHandler, IScrollHandler
{
    [SerializeField]
    private RectTransform _boardTransform;
    public RectTransform BoardTransform => _boardTransform;

    [Header("Input")]
    [SerializeField] private float _zoomSpeed = 0.05f;
    [SerializeField] private float _zoomOutLimit = 0.328f;
    [SerializeField] private float _zoomInLimit = 1.25f;

    private Vector2 _boardCenter;
    private Rect _boardBoundsRect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        InitializeSingleton();
    }

    void Start()
    {
        RectTransform mask = _boardTransform.parent as RectTransform;
        _boardBoundsRect = mask.rect;
    }

    // Update is called once per frame
    void Update()
    {
        _boardCenter = _boardTransform.parent.position;
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
        }
        else if (topRight.y < boardMax.y)
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

    public void OnDrag(PointerEventData eventData)
    {
        _boardTransform.anchoredPosition += eventData.delta;
        ClampBoard();
    }
}
