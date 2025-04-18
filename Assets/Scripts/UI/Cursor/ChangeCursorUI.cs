using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCursorUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ModeOfCursor modeOfCursor;

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
        //CursorManager.Instance.SetToMode(modeOfCursor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //CursorManager.Instance.SetToMode(ModeOfCursor.Default);
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