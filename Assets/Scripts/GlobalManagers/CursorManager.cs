using UnityEngine;

namespace GlobalManagers
{
    public class CursorManager : MonoBehaviour
    {
        
        public static CursorManager Instance { get; private set; }
        
        [SerializeField] private Texture2D cursorTextureDefault;
        [SerializeField] private Texture2D cursorTextureAction;
        [SerializeField] private Texture2D cursorTextureInspect;
        
        [SerializeField] private Vector2 clickPosition = Vector2.zero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
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
}
