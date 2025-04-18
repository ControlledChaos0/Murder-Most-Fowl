using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField] private Texture2D cursorTextureDefault;
    [SerializeField] private Texture2D cursorTextureAction;
    [SerializeField] private Texture2D cursorTextureInspect;
        
    [SerializeField] private Vector2 clickPosition = Vector2.zero;

    private void Awake()
    {
        InitializeSingleton();
    }
        
    private void Start()
    {
        Cursor.SetCursor(cursorTextureDefault, clickPosition, CursorMode.Auto);
    }

    public void SetToMode(ModeOfCursor modeOfCursor)
    {
        switch (modeOfCursor)
        {
            case ModeOfCursor.Default:
                Cursor.SetCursor(cursorTextureDefault, clickPosition, CursorMode.Auto);
                break;
            case ModeOfCursor.Action:
                Cursor.SetCursor(cursorTextureAction, clickPosition, CursorMode.Auto);
                break;
            case ModeOfCursor.Inspect:
                Cursor.SetCursor(cursorTextureInspect, clickPosition, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(cursorTextureDefault, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}
    
public enum ModeOfCursor
{
    Default,
    Action,
    Inspect
}
