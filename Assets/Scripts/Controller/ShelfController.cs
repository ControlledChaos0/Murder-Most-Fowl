using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShelfController : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    Sprite openSprite;
    [SerializeField]
    Sprite closedSprite;
    [SerializeField]
    BoxCollider closedCollider;
    [SerializeField]
    BoxCollider openCollider1;
    [SerializeField]
    BoxCollider openCollider2;
    [SerializeField]
    private Texture2D _normalCursor;
    [SerializeField]
    private Texture2D _magnifyingCursor;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
         Cursor.SetCursor(_magnifyingCursor, Vector2.zero, CursorMode.Auto);
         hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
        hovered = false;
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
