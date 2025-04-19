using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCursor : Button
{
    private bool _hovered = false;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Action);
        _hovered = true;
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Default);
        _hovered = false;
        base.OnPointerExit(eventData);
    }

    protected override void OnDisable()
    {

        if (_hovered)
        {
            CursorManager.Instance.SetToMode(ModeOfCursor.Default);
            _hovered = false;
        }
        base.OnDisable();
    }

}
