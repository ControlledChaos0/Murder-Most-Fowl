using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShelfController : MonoBehaviour,
    IPointerDownHandler
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
    private bool open = false;
    SpriteRenderer sr =  null;

    private bool hovered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;
        openCollider1.enabled = false;
        openCollider2.enabled = false;
        closedCollider.enabled = true;
        open = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (open)
        {
            sr.sprite = closedSprite;
            openCollider1.enabled = false;
            openCollider2.enabled = false;
            closedCollider.enabled = true;
            open = false;
        } else {
            sr.sprite = openSprite;
            openCollider1.enabled = true;
            openCollider2.enabled = true;
            closedCollider.enabled = false;
            open = true;
        }
    }

    public bool IsOpen() 
    {
        return open;
    }

    public bool isOpen()
    {
        return open;
    }

    void OnDisable()
    {
        if (hovered)
        {
            CursorManager.Instance.SetToMode(ModeOfCursor.Default);
            hovered = false;
        }
    }
}
