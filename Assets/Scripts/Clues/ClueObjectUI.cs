using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Clues
{
    public class ClueObjectUI : MonoBehaviour, 
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IScrollHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Clue _clue;

        private Vector2 _offset;
        private bool _mouseDown;

        private static readonly float _sizeMin = .5f;
        private static readonly float _sizeMax = 2.5f;
        Vector3 _scaleChange;

        // Start is called before the first frame update
        private void Start()
        {
            transform.parent = ClueBoardManager.Instance.HoldingPinTransform;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            ClueBoardManager.Instance.AddToBin(this);
            _mouseDown = false;
            _scaleChange = new Vector3(0.2f, 0.2f, 0.2f);
        }

        public void AddClue(Clue clue)
        {
            _clue = clue;
            _spriteRenderer.sprite = clue.Icon;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position + _offset;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector2 mousePos = eventData.pressPosition;
            Vector2 uiPos = transform.position;
            _offset = uiPos - mousePos;

            transform.parent = ClueBoardManager.Instance.Clues;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _offset = Vector2.zero;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (_mouseDown)
            {
                GameObject sprite = _spriteRenderer.gameObject;
                // Increases and decreases size of sprite
                var w = Input.mouseScrollDelta.y;
                if (w > 0 && _sizeMax > sprite.transform.localScale.x) {
                    sprite.transform.localScale += _scaleChange;
                } else if (w < 0 && _sizeMin < sprite.transform.localScale.x) {
                    sprite.transform.localScale -= _scaleChange;
                }
            }
            else
            {
                ClueBoardManager.Instance.OnScroll(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _mouseDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _mouseDown = false;
        }
    }
}
