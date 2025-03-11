using GlobalManagers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class CustomCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ModeOfCursor modeOfCursor;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorManager.Instance.SetToMode(modeOfCursor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorManager.Instance.SetToMode(ModeOfCursor.Default);
        }
    }
}
