using GlobalManagers;
using UnityEngine;

namespace UI
{
    public class ChangeCursorWorld : MonoBehaviour
    {
        [SerializeField] private ModeOfCursor modeOfCursor;
        
        private void Awake()
        {
            #if UNITY_EDITOR
            ValidateCursorUsage();
            #endif
        }
        
        private void OnMouseEnter()
        {
            CursorManager.Instance.SetToMode(modeOfCursor);
        }

        private void OnMouseExit()
        {
            CursorManager.Instance.SetToMode(ModeOfCursor.Default);
        }
        
        #if UNITY_EDITOR
        private void ValidateCursorUsage()
        {
            var hasCollider = GetComponent<Collider>() != null || GetComponent<Collider2D>() != null;
            
            if (!hasCollider)
            {
                Debug.LogError($"[ChangeCursorUI] {gameObject.name} is using ChangeCursorWorld but it doesn't have a collider! Add a collider, or if it is a UI element, use ChangeCursorUI instead.");
            }
        }
        #endif
    }
}