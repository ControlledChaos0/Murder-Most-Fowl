using Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayerInteractable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    protected float radiusToStop = 1.0f;
    public float Radius
    {
        get => radiusToStop;
        set => radiusToStop = value;
    }

    public bool WithinRange(Vector3 pos)
    {
        return Mathf.Abs(pos.x - transform.position.x) <= radiusToStop;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClick();
    }

    protected abstract void OnPointerClick();
}

public abstract class ToggleInteractable : PlayerInteractable
{
    [SerializeField]
    protected bool _isToggled = false;
    protected abstract void ToggleOn();
    protected abstract void ToggleOff();

    public virtual void Toggle()
    {
        if (_isToggled)
        {
            ToggleOff();
            _isToggled = false;
        }
        else
        {
            ToggleOn();
            _isToggled = true;
        }
    }
    protected override void OnPointerClick()
    {
        CommandManager.Instance.Queue(new ToggleCommand(this));
    }
}