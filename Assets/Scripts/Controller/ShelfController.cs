using Unity.VisualScripting;
using UnityEngine;

public class CabinetController : MonoBehaviour
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;
        openCollider1.enabled = false;
        openCollider2.enabled = false;
        open = false;
    }

    void OnMouseDown()
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

    void OnMouseEnter()
    {
         Cursor.SetCursor(_magnifyingCursor, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
    }

    public bool IsOpen() 
    {
        return open;
    }
}
