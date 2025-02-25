using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Clues
{
    public class ClueObjectUI : MonoBehaviour, 
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IScrollHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Vector2 _offset;
        private Vector3 _worldOffset;
        private bool _mouseDown;
        private bool _scaling;

        public GameObject _sprite;

        private static readonly float _sizeMin = .5f;
        private static readonly float _sizeMax = 3.0f;
        private Vector3 _initialScale;
        private Vector3 _scaleChange;

        // Start is called before the first frame update
        private void Start()
        {
            transform.parent = ClueBoardManager.Instance.HoldingPinTransform;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            ClueBoardManager.Instance.AddToBin(this);
            _mouseDown = false;
            _scaleChange = new Vector3(0.2f, 0.2f, 0.2f);
            _scaling = false;
            _initialScale = Vector3.one;
        }

        // TODO: Never used. remove?
        private void StartDelay()
        {
            ClueBoardManager.Instance.AddToBin(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_scaling == true){
                Vector3 mousePosWorld = Camera.main.WorldToScreenPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
                Vector3 uiPosWorld = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));
                Vector3 newOffset = uiPosWorld - mousePosWorld;
                
                var change = newOffset.magnitude/_worldOffset.magnitude;
                if ((_sizeMax > _sprite.transform.localScale.x) && (newOffset.magnitude > _worldOffset.magnitude)) {
                    // Scale up
                    if (_initialScale.x * change > _sizeMax){
                        // reach maximum size
                        _sprite.transform.localScale = new Vector3(_sizeMax, _sizeMax, 1);
                    } else {
                        _sprite.transform.localScale = _initialScale * change; 
                    }
                } else if (_sizeMin < _sprite.transform.localScale.x && newOffset.magnitude < _worldOffset.magnitude){
                    // Scale Down
                    if (_initialScale.x * change < _sizeMin){
                        // reach minimum size
                        _sprite.transform.localScale = new Vector3(_sizeMin, _sizeMin, 1);

                    } else {
                        _sprite.transform.localScale = _initialScale * change; 
                    }
                    
                }
                
            } else {
                transform.position = eventData.position + _offset;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector2 mousePos = eventData.pressPosition;
            Vector2 uiPos = transform.position;
            _offset = uiPos - mousePos;

            Vector3 mousePosWorld = Camera.main.WorldToScreenPoint(new Vector3(mousePos.x, mousePos.y, 0));
            Vector3 uiPosWorld = Camera.main.WorldToScreenPoint(new Vector3(uiPos.x, uiPos.y, 0));
            _worldOffset = uiPosWorld - mousePosWorld;

            transform.parent = ClueBoardManager.Instance.Clues;

            var renderer = _sprite.GetComponent<RectTransform>();
            var width = renderer.rect.width * _sprite.transform.localScale.x;
            var height = renderer.rect.height * _sprite.transform.localScale.y;
            var margin = 5;

            if (_offset.x > (width/2 - margin) * 0.6f || _offset.y > (height/2 - margin) * 0.6f || _offset.x < (-width/2 + margin) * 0.6f || _offset.y < (-height/2 + margin) * 0.6f) {
                _scaling = true;
                _initialScale = _sprite.transform.localScale;
            } else {
                _scaling = false;
            }
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _offset = Vector2.zero;
            _scaling = false;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (_mouseDown)
            {
                // Increases and decreases size of sprite
                var w = Input.mouseScrollDelta.y;
                if (w > 0 && _sizeMax > _sprite.transform.localScale.x) {
                    _sprite.transform.localScale += _scaleChange;
                } else if (w < 0 && _sizeMin < _sprite.transform.localScale.x) {
                    _sprite.transform.localScale -= _scaleChange;
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
