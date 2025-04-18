using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCursorWorld : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
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
        var hasCollider = GetComponent<Collider>() != null || GetComponent<Collider2D>() != null;
        
        if (!hasCollider)
        {
            Debug.LogError($"[ChangeCursorUI] {gameObject.name} is using ChangeCursorWorld but it doesn't have a collider! Add a collider, or if it is a UI element, use ChangeCursorUI instead.");
        }
    }
    #endif
}