using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ModeOfCursor modeOfCursor;

    private bool _hovered = false;
    public ModeOfCursor ModeOfCursor
    {
        get => modeOfCursor;
        set => modeOfCursor = value;
    }

    private void Awake()
    {
        #if UNITY_EDITOR
        ValidateCursorUsage();
        #endif
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(modeOfCursor);
        _hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Default);
        _hovered = false;
    }

    void OnDisable()
    {
        if (_hovered)
        {
            CursorManager.Instance.SetToMode(ModeOfCursor.Default);
            _hovered = false;
        }
    }
    
    #if UNITY_EDITOR
    private void ValidateCursorUsage()
    {
        var hasUIComponent = GetComponent<RectTransform>() != null;

        if (!hasUIComponent)
        {
            Debug.LogError($"[ChangeCursorUI] {gameObject.name} is using ChangeCursorUI but is not a UI element! Use ChangeCursorWorld instead.");
        }
    }
    #endif
}