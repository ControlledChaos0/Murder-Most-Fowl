using System.Collections.Generic;
using Clues;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClueBoardBin : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerClickHandler
{
    [SerializeField] protected RectTransform _menuTransform;
    [SerializeField] protected Animator _animator;

    protected List<ClueObjectUI> _clueList;

    protected ClueObjectUI _draggedClue;
    protected bool _showMenu;

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
        //RectTransform boardTransform = ClueBoardManager.Instance.BoardTransform;
        clueObject.gameObject.transform.parent = ClueBoardManager.Instance.Clues;
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
        _draggedClue?.OnEndDrag(eventData);
        RemoveFromBin(_draggedClue);
        _draggedClue = null;
    }

    public void AddToBin(ClueObjectUI clueObject)
    {
        _clueList.Add(clueObject);
    }

    public void RemoveFromBin(ClueObjectUI clueObject)
    {
        if (!clueObject)
        {
            return;
        }

        _clueList.Remove(clueObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleBin();
    }

    public void InitBin()
    {
        _menuTransform.sizeDelta = Vector2.zero;
        _menuTransform.anchoredPosition = Vector2.zero;
    }

    protected void ToggleBin()
    {
        if (_showMenu)
        {
            CloseBin();
        }
        else
        {
            OpenBin();
        }
    }

    protected void OpenBin()
    {
        _showMenu = true;
        _animator.Play("Reveal");
    }

    protected void CloseBin()
    {
        _showMenu = false;
        _animator.Play("Hide");
    }
}
