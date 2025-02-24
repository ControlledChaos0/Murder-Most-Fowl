using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PseudoDataStructures
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class SubcanvasMask : UIBehaviour
    {
        [SerializeField] private CanvasRenderer _canvas;
        [SerializeField] private RectMask2D _mask;
        
        private Canvas rootCanvas = null;
        private RectTransform maskRectTransform = null;
        private bool initialized = false;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if( initialized )
            {
                SetTargetClippingRect();
            }
        }

        protected override void Awake()
        {
            //_canvas = GetComponent<CanvasRenderer>();
            rootCanvas = transform.parent.GetComponentInParent<Canvas>();
            maskRectTransform = GetComponentInParent<Mask>().rectTransform;
            SetTargetClippingRect();
            initialized = true;
        }

        private void SetTargetClippingRect()
        {
            Rect rect = maskRectTransform.rect;
            // Get local position of maskRect as if it was direct child of root canvas, then offset mask rect by that amount
            //rect.center += (Vector2)rootCanvas.transform.InverseTransformPoint( maskRectTransform.position );
            print(rect);
            //rect = new Rect(0, 0, 0.01f, 0.01f);
            //_mask.rectTransform.rect = rect;
            //_canvas.EnableRectClipping(rect);
        }
    }
}