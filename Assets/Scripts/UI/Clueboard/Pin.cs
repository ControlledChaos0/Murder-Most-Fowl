using System;
using System.Collections.Generic;
using Clues;
using UnityEngine;
using UnityEngine.EventSystems;

// using namespace UI.Clueboard;

public class Pin : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    //private UILineRenderer _lineRenderer;
    private ClueString clueString;

    private Pin _connected;
    
    private void Awake()
    {
        clueString = GetComponentInChildren<ClueString>();
    }
    
    bool _isDragging;

    void Update()
    {
        clueString.transform.position = transform.position;
        if (!_isDragging && _connected != null)
        {
            SetLineRendererEnd(_connected.transform.position);
        }
    }

    void SetLineRendererEnd(Vector2 endPos)
    {
        endPos = (endPos - (Vector2) transform.position) / (ClueBoard.Instance.BoardTransform.localScale.x * ClueBoardManager.Instance.transform.localScale.x);
        clueString.SetEndPoint(endPos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetLineRendererEnd(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        _connected = null;
        
        foreach (var raycastResult in raycastResults)
        {
            var pin = raycastResult.gameObject.GetComponent<Pin>();

            if (pin != null)
            {
                _connected = pin;
                break;
            }
        }

        if (_connected == null)
        {
            clueString.SetEndPoint(Vector2.zero);
        }
    }
}