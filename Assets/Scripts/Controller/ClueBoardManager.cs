using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Clues;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Yarn.Unity;

public class ClueBoardManager : Singleton<ClueBoardManager>
{
    [Header("Transforms")]
    [SerializeField]
    private Canvas _canvas;
    public Canvas ClueBoardCanvas => _canvas;
    [SerializeField] 
    private Animator _animator;
    [SerializeField]
    private RectTransform _holdingPinTransform;
    public RectTransform HoldingPinTransform => _holdingPinTransform;

    [SerializeField] private RectTransform stringRenderers;
    public RectTransform StringRenderers => stringRenderers;
    
    [SerializeField] private RectTransform clues;
    public RectTransform Clues => clues;
    [SerializeField] private RectTransform _front;
    public RectTransform Front => _front;

    [Header("Clue UI")]
    [SerializeField]
    private GameObject _objectUI;

    [Header("Sub-objects")]
    [SerializeField]
    private NewBin _newBin;
    public NewBin NewBin => _newBin;
    [SerializeField]
    private ClueBoardDisplay _display;
    [SerializeField]
    private ClueboardButton _toggleButton;
    [SerializeField]
    private GameObject _stickyNote;

    private ClueObjectUI _selected;
    public ClueObjectUI Selected
    {
        get => _selected;
        set => _selected = value;
    }

    public bool InClueboard
    {
        get => _activated;
    }

    public bool ToggleLock
    {
        get => _toggleLock;
        set => _toggleLock = value;
    }

    private GraphicRaycaster _graphicRaycaster;

    public GraphicRaycaster GraphicRaycast => _graphicRaycaster;
    //private ClueObjectUI _selectedObj;

    private Vector2 _boardCenter;
    private Rect _boardBoundsRect;

    private bool _toggleLock;
    private bool _activated;
    //private bool _scrollEnabled;
    private bool _canPresent;

    private string _correctEvidence;
    private string _incorrectNode;


    private readonly Vector2 DEFAULT_PIVOT = new(0.5f, 0.5f);
    private bool _spawnable;

    public bool CanPresent => _canPresent;

    private void Awake() {
        InitializeSingleton();
        _toggleLock = false;
        _activated = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _graphicRaycaster = GetComponent<GraphicRaycaster>();

        InputController.Instance.ToggleClueBoard += ToggleClueBoard;
        InputController.Instance.Click += SetSelectedClueObject;
        // InputController.Instance.ToggleStickyNote += ToggleStickyNote;
        //_scrollEnabled = true;
        _canPresent = false;

        _spawnable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        InputController.Instance.ToggleClueBoard -= ToggleClueBoard;
        InputController.Instance.Click -= SetSelectedClueObject;
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
        _activated = true;
        _toggleButton.OpenClueBoard();
        _animator.Play("Reveal");
    }

    private void CloseClueBoard()
    {
        Selected?.OnDeselect(null);
        _activated = false;
        _canPresent = false;
        _toggleButton.CloseClueBoard();
        _animator.Play("Hide");
    }

    public void LockToggle()
    {
        _toggleLock = true;
        _toggleButton.gameObject.SetActive(false);
    }

    public void UnlockToggle()
    {
        _toggleLock = false;
        _toggleButton.gameObject.SetActive(true);
    }

    [YarnCommand("CreateClue")]
    public void InstantiateClue(string clueID)
    {
        if (GameManager.State.DiscoveredClues.Contains(clueID))
        {
            return;
        }

        GameManager.State.DiscoveredClues.Add(clueID);
        GameObject clueObject = Instantiate(_objectUI);
        ClueObjectUI clueUI = clueObject.GetComponent<ClueObjectUI>();

        clueUI.AddClue(clueID);
    }

    public void SetSelectedClueObject(PointerEventData pointerEvent)
    {
        if (!InClueboard)
        {
            return;
        }

        if (!pointerEvent.pointerClick)
        {
            Selected?.OnDeselect(pointerEvent);
            return;
        }

        ClueObjectUI selectedUI = pointerEvent.pointerClick.GetComponentInParent<ClueObjectUI>();
        if (selectedUI)
        {
            if (!Selected || (Selected && !Selected.Equals(selectedUI)))
            {
                Selected?.OnDeselect(pointerEvent);
                Selected = selectedUI;
                Selected.OnSelect(pointerEvent);
            }
        }
        else
        {
            Selected?.OnDeselect(pointerEvent);
        }
    }

    public void ChangeDisplay(string clueID = null)
    {
        _display.SetDisplay(clueID);
    }

    // Sticky note function
    //public void OnPointerDown(PointerEventData eventData)
    //{
        //if (!_spawnable) return;
        //var spawnPosition = new Vector3(eventData.position.x, eventData.position.y, 0);
        //var parent = ClueBoardManager.Instance.BoardTransform;
        //Instantiate(_stickyNote, spawnPosition, Quaternion.identity, parent);
        //_spawnable = false;
    //}

    public void ToggleStickyNote()
    {
        _spawnable = !_spawnable;
    }

    [YarnCommand("PresentEvidence")]
    public static void InitPresentEvidence(string clueID = "", string incorrectNode = "")
    {
        if (!string.IsNullOrEmpty(clueID))
        {
            Instance._correctEvidence = clueID;
            Instance._incorrectNode = incorrectNode;
        }
        else
        {
            Instance._correctEvidence = null;
        }

        Instance.OpenClueBoard();
        Instance._canPresent = true;
    }

    public void PresentEvidence(Clue clue)
    {
        if (!_canPresent)
        {
            return;
        }

        CloseClueBoard();
        string node = GameManager.CharacterManager.GetClueResponse(clue.ClueID);
        if (!string.IsNullOrEmpty(_correctEvidence) && _correctEvidence != clue.ClueID)
        {
            node = _incorrectNode;
        }
        if (!string.IsNullOrEmpty(node))
        {
            DialogueHelper.Instance.DialogueRunner.StartDialogue(node);
        }
    }
}
