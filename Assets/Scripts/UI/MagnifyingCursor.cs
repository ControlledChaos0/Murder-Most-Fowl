using UnityEngine;

public class MagnifyingCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D _normalCursor;
    [SerializeField]
    private Texture2D _magnifyingCursor;
    [SerializeField]
    private bool onlyMain = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        if (!onlyMain || !GameManager.StateManager.ActiveState.Tutorial)
        {
            Cursor.SetCursor(_magnifyingCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
    }
}
