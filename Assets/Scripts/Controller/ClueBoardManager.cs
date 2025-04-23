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
    [SerializeField] private RectTransform _inspectScreen;
    public RectTransform InspectScreen => _inspectScreen;

    [Header("Clue UI")]
    [SerializeField]
    private GameObject _objectUI;

    [Header("Sub-objects")]
    [SerializeField]
    private NewBin _newBin;
    public NewBin NewBin => _newBin;
    [SerializeField]
    private ClueBoard _clueBoard;
    [SerializeField]
    private ClueBoardDisplay _display;
    [SerializeField]
    private ClueboardButton _toggleButton;

    [SerializeField]
    private GameObject _presentButton;

    public GameObject PresentButton
    {
        get => _presentButton;
    }

    [SerializeField] 
    private GameObject _inspectButton;

    public GameObject InspectButton
    {
        get => _inspectButton;
    }

    [SerializeField]
    private Image _viewableImage;
    [SerializeField]
    private GameObject _stickyNote;

    private ClueObjectUI _selectedClue;
    public ClueObjectUI SelectedClue
    {
        get => _selectedClue;
        set => _selectedClue = value;
    }

    private ClueObjectUI _prevSelectedClue;

    private ClueString _selectedString;
    public ClueString SelectedString
    {
        get => _selectedString;
        set => _selectedString = value;
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
        InputController.Instance.Click += SetSelectedString;
        // InputController.Instance.ToggleStickyNote += ToggleStickyNote;
        //_scrollEnabled = true;
        _canPresent = false;

        _spawnable = false;
        _presentButton.SetActive(false);
        _inspectButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_prevSelectedClue != SelectedClue)
        {
            _presentButton.SetActive(SelectedClue != null && _canPresent);
            _inspectButton.SetActive(SelectedClue != null && SelectedClue.Clue.Viewable);
        }

        if (SelectedClue)
        {
            _prevSelectedClue = SelectedClue;
        }
    }

    void OnDestroy()
    {
        InputController.Instance.ToggleClueBoard -= ToggleClueBoard;
        InputController.Instance.Click -= SetSelectedClueObject;
        InputController.Instance.Click -= SetSelectedString;
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
        SelectedClue?.OnDeselect(null);
        SelectedString?.OnDeselect(null);
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
        if (!InClueboard || !pointerEvent.pointerClick)
        {
            return;
        }

        ClueObjectUI selectedUI = pointerEvent.pointerClick.GetComponentInParent<ClueObjectUI>();
        if (selectedUI)
        {
            if (!SelectedClue || (SelectedClue && !SelectedClue.Equals(selectedUI)))
            {
                SelectedClue?.OnDeselect(pointerEvent);
                SelectedClue = selectedUI;
                SelectedClue.OnSelect(pointerEvent);
            }
        }
        else if (pointerEvent.pointerClick.Equals(_clueBoard.BoardTransform.gameObject))
        {
            SelectedClue?.OnDeselect(pointerEvent);
        }
    }

    public void SetSelectedString(PointerEventData pointerEvent)
    {
        if (!InClueboard)
        {
            return;
        }

        if (!pointerEvent.pointerClick)
        {
            SelectedString?.OnDeselect(pointerEvent);
            return;
        }

        ClueString selectedString = pointerEvent.pointerClick.GetComponentInParent<ClueString>();
        if (selectedString)
        {
            if (!SelectedString || (SelectedString && !SelectedString.Equals(selectedString)))
            {
                SelectedString?.OnDeselect(pointerEvent);
                SelectedString = selectedString;
                SelectedString.OnSelect(pointerEvent);
            }
        }
        else
        {
            SelectedString?.OnDeselect(pointerEvent);
        }
    }

    public void ChangeDisplay(string clueID = null)
    {
        tempClueID = clueID;
        CoroutineUtils.ExecuteAfterEndOfFrame(ChangeDisplayWait, this);
    }

    private string tempClueID;
    public void ChangeDisplayWait()
    {
        _display.SetDisplay(tempClueID);
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

    public void OpenInspectScreen()
    {
        _viewableImage.sprite = _prevSelectedClue.Clue.ViewSprite;
        _viewableImage.preserveAspect = true;
        InspectScreen.gameObject.SetActive(true);
    }

    public void CloseInspectScreen()
    {
        InspectScreen.gameObject.SetActive(false);
    }
}
