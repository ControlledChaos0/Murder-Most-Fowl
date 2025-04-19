using System;
using System.Collections.Generic;
using Clues;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// using namespace UI.Clueboard;

public class Pin : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    //private UILineRenderer _lineRenderer;
    [SerializeField]
    private GameObject stringPrefab;

    private ClueString currString;
    private Pin _connected;

    public List<ClueString> ClueStrings;
    
    private void Awake()
    {
        ClueStrings = new List<ClueString>();
        //clueString = GetComponentInChildren<ClueString>();
    }
    
    bool _isDragging;

    void Update()
    {

    }

    void OnEnable()
    {
        ClueStrings.Clear();
    }

    void OnDisable()
    {
        int count = ClueStrings.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(ClueStrings[i].gameObject);
        }
    }

    void SetLineRendererEnd(ClueString clueString, Vector2 endPos)
    {
        endPos = (endPos - (Vector2) transform.position) / (ClueBoard.Instance.BoardTransform.localScale.x * ClueBoardManager.Instance.transform.localScale.x);
        clueString.SetEndPoint(endPos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        GameObject stringGO = Instantiate(stringPrefab);
        stringGO.transform.parent = ClueBoardManager.Instance.StringRenderers;
        stringGO.transform.localScale = Vector2.one;

        currString = stringGO.GetComponent<ClueString>();
        currString.Pins[0] = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        currString.transform.position = transform.position;
        SetLineRendererEnd(currString, eventData.position);
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
                currString.Pins[1] = _connected;
                break;
            }
        }

        if (_connected == null || currString.CheckIfDuplicate())
        {
            Destroy(currString.gameObject);
            currString = null;
            return;
        }

        ClueStrings.Add(currString);
        currString.Pins[1].ClueStrings.Add(currString);
        currString = null;
    }

    public void RemoveString(ClueString clueString)
    {
        ClueStrings.Remove(clueString);
    }
}