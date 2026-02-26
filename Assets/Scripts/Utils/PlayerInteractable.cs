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
