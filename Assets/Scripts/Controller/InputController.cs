using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputController : Singleton<InputController>
{
    [SerializeField]
    private InputActionAsset inputActions;
    [SerializeField]
    private EventSystem eventSystem;
    [SerializeField]
    private InputSystemUIInputModule inputModule;

    [SerializeField]
    private float _pressTime;

    private InputActionMap _currentActionMap;
    
    private InputActionMap _mainControls;
    //private InputActionMap _clueBoardControls;
    private InputActionMap _uiControls;

    private InputAction _pointAction;
    private InputAction _clickAction;
    private InputAction _rightClickAction;

    private InputAction _scrollAction;
    //private InputAction _cancelAction;

    //private InputAction _moveAction;
    private InputAction _clueBoardAction;
    //private InputAction _scrollClueBoardAction;

    //private Vector2 _mouseDelta;
    //private Vector2 _screenPosition;
    //private float _scrollDelta;
    private bool _clickDown;
    private bool _rightClickDown;

    private float _clickTime;
    private float _rightClickTime;
    //private bool _moveCamera;
    //private bool _panCamera;

    //public Vector2 ScreenPosition {
    //    get { return _screenPosition; }
    //}

    ////Events
    public event Action<PointerEventData> Click;
    public event Action Hold;
    public event Action Cancel;
    public event Action RightClick;
    public event Action RightHold;
    public event Action RightCancel;
    //public event Action<Vector2> Hover;
    //public event Action<Vector2> MouseMove;

    public event Action ToggleClueBoard;
    //public event Action<float> OnScrollCB;

    private void Awake()
    {
        InitializeSingleton();

        SetControls();
        InitializeControls();

        //Overall Controls
        _mainControls.Enable();
    }
    //// Start is called before the first frame update
    void Start()
    {
        _clickDown = false;
        _rightClickDown = false;
    }

    //// Update is called once per frame
    void Update()
    {
        
    }

    private void SetControls()
    {
        SetUIControls();
        SetMainControls();
    }

    private void InitializeControls()
    {
        InitializeUIControls();
        InitializeMainControls();
    }

    private void SetMainControls() {
        _mainControls = inputActions.FindActionMap("MainControls");

        _clueBoardAction = _mainControls.FindAction("ClueBoard");
    }

    private void SetUIControls()
    {
        _uiControls = inputActions.FindActionMap("UI");

        _rightClickAction = _uiControls.FindAction("RightClick");
        _clickAction = _uiControls.FindAction("Click");
        _pointAction = _uiControls.FindAction("Point");
    }

    private void InitializeMainControls()
    {
        _clueBoardAction.performed += OnToggleClueBoard;
    }

    private void InitializeUIControls()
    {
        _rightClickAction.performed += OnRightClickPerformed;
        _clickAction.performed += OnClickPerformed;
    }

    //private void OnEnable() {
    //    inputActions.FindActionMap(_currentActionMap.id).Enable();
    //}

    //private void OnDisable() {
    //    inputActions.FindActionMap(_currentActionMap.id).Disable();
    //}

    //private void OnDeltaCursor(InputAction.CallbackContext inputValue) {
    //    _mouseDelta = inputValue.ReadValue<Vector2>();
    //    MouseMove?.Invoke(_mouseDelta);
    //}

    //private void OnCancel(InputValue inputValue) {
    //    Cancel?.Invoke();
    //}
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (_clickDown)
        {
            OnClickUp();
        }
        else
        {
            OnClickDown();
        }
    }
    private void OnClickDown()
    {
        Debug.Log("OnClickDown");
        _clickDown = true;
        _clickTime = Time.time;
    }
    private void OnClickUp()
    {
        Debug.Log("OnClickUp");
        _clickDown = false;
        if (Time.time - _clickTime >= _pressTime)
        {
            //TODO
            //Rework Hold to take into account both time and position
            //Probably should sit in Update or somewhere else
            return;
        }
        Debug.Log("Just a click!!!");
        Vector2 screenPos = _pointAction.ReadValue<Vector2>();
        PointerEventData eventData = CameraController.Instance.Raycast(screenPos);
        if (eventData != null)
        {
            Click?.Invoke(eventData);
        }
    }

    //TODO
    //Redo RightClick action if needed in line with LeftClick
    private void OnRightClickStarted(InputAction.CallbackContext context)
    {
        _rightClickTime = Time.time;
    }
    private void OnRightClickPerformed(InputAction.CallbackContext context)
    {
        RightHold?.Invoke();
    }
    private void OnRightClickCanceled(InputAction.CallbackContext context)
    {
        if (Time.time - _rightClickTime >= _pressTime)
        {
            RightCancel?.Invoke();
            return;
        }
        RightClick?.Invoke();
        Debug.Log("Just a right click!!!");
    }

    private void OnToggleClueBoard(InputAction.CallbackContext context)
    {
        ToggleClueBoard?.Invoke();
    }

    public void DisableClueBoardInput()
    {
        Debug.Log("DisableClueBoardInput");
        _clueBoardAction.Disable();
    }

    public void EnableClueBoardInput()
    {
        Debug.Log("EnableClueBoardInput");
        _clueBoardAction.Enable();
    }
    
    //private void OnCloseClueBoard(InputAction.CallbackContext context) {
    //    ToggleClueBoard?.Invoke(false);
    //}

    //public void ToggleControls(bool enabled) {
    //    if (enabled) {
    //        ToggleActionMap(_clueBoardControls);
    //    } else {
    //        ToggleActionMap(_mainControls);
    //    }
    //}

    //private void OnScrollPerformed(InputAction.CallbackContext context) {
    //    //Debug.Log(context.ReadValue<Vector2>().y);
    //    OnScrollCB?.Invoke(context.ReadValue<Vector2>().y);
    //}

    //private void ToggleActionMap(InputActionMap actionMap) {
    //    if (actionMap.enabled) {
    //        return;
    //    }

    //    _currentActionMap.Disable();
    //    _currentActionMap = actionMap;
    //    _currentActionMap.Enable();
    //}

}
