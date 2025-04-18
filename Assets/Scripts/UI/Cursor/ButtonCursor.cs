using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCursor : Button
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Action);
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Default);
        base.OnPointerExit(eventData);
    }
}
