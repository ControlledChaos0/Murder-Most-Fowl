using Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShelfController : ToggleInteractable
{
    [SerializeField]
    Sprite openSprite;
    [SerializeField]
    Sprite closedSprite;
    [SerializeField]
    BoxCollider2D closedCollider;
    [SerializeField]
    BoxCollider2D openCollider1;
    [SerializeField]
    BoxCollider2D openCollider2;
    SpriteRenderer sr =  null;

    private bool hovered = false;

    public bool IsOpen
    {
        get => _isToggled;
        private set => _isToggled = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;
        openCollider1.enabled = false;
        openCollider2.enabled = false;
        closedCollider.enabled = true;
        IsOpen = false;
    }    

    void OnDisable()
    {
        if (hovered)
        {
            CursorManager.Instance.SetToMode(ModeOfCursor.Default);
            hovered = false;
        }
    }

    protected override void ToggleOn()
    {
        sr.sprite = openSprite;
        openCollider1.enabled = true;
        openCollider2.enabled = true;
        closedCollider.enabled = false;
    }

    protected override void ToggleOff()
    {
        sr.sprite = closedSprite;
        openCollider1.enabled = false;
        openCollider2.enabled = false;
        closedCollider.enabled = true;
    }
}
