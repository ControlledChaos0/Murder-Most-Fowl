using System.Collections.Generic;
using Clues;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClueBoardBin : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerClickHandler
{
    [SerializeField] private RectTransform _menuTransform;
    [SerializeField] private Animator _animator;

    private List<ClueObjectUI> _clueList;

    private ClueObjectUI _draggedClue;
    private bool _showMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _clueList = new List<ClueObjectUI>();
    }

    // Update is called once per frame
    void Update()
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
        GameManager.StateManager.ActiveState.NewClueBin.Add(clueObject.Clue.ClueID);
    }

    public void RemoveFromBin(ClueObjectUI clueObject)
    {
        if (!clueObject)
        {
            return;
        }

        _clueList.Remove(clueObject);
        GameManager.StateManager.ActiveState.NewClueBin.Remove(clueObject.Clue.ClueID);
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

    private void ToggleBin()
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

    private void OpenBin()
    {
        _showMenu = true;
        _animator.Play("Reveal");
    }

    private void CloseBin()
    {
        _showMenu = false;
        _animator.Play("Hide");
    }
}
