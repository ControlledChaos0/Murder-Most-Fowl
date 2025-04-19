using System.Collections.Generic;
using Clues;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClueBoardBin : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Bin Transforms")]
    [SerializeField] protected RectTransform _storageTransform;

    [SerializeField] protected internal List<ClueObjectUI> _clueList;

    protected ClueObjectUI _draggedClue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        _clueList = new List<ClueObjectUI>();
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start Drag");
        if (_clueList.Count <= 0)
        {
            return;
        }

        ClueObjectUI clueObject = _clueList[0];
        _clueList.RemoveAt(0);

        clueObject.gameObject.transform.position = eventData.position;
        clueObject.gameObject.transform.localScale = Vector3.one;
        eventData.pressPosition = eventData.position;
        _draggedClue = clueObject;

        _draggedClue.OnBeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        _draggedClue?.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        RemoveFromBin(_draggedClue);
        _draggedClue?.OnEndDrag(eventData);
        _draggedClue = null;
    }

    public virtual void AddToBin(ClueObjectUI clueObject)
    {
        _clueList.Add(clueObject);
        clueObject.transform.parent = _storageTransform;
        clueObject.transform.localPosition = Vector3.zero;
        clueObject.transform.localScale = Vector3.one;
        clueObject.Image.transform.localScale = Vector3.one;
    }

    public virtual void RemoveFromBin(ClueObjectUI clueObject)
    {
        if (!clueObject)
        {
            return;
        }

        _clueList.Remove(clueObject);
    }
}
