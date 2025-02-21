using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Clues
{
    public class ClueObjectUI : MonoBehaviour, 
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IScrollHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _image;

        private Clue _clue;
        private ClueBoardClue _saveClue;
        private bool _onBoard;

        private Vector2 _offset;
        private bool _mouseDown;

        private static readonly float _sizeMin = .5f;
        private static readonly float _sizeMax = 2.5f;
        Vector3 _scaleChange;

        public Clue Clue
        {
            get => _clue;
            private set => _clue = value;
        }

        // Start is called before the first frame update
        private void Start()
        {
            transform.parent = ClueBoardManager.Instance.HoldingPinTransform;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            ClueBoardManager.Instance.AddToBin(this);
            _mouseDown = false;
            _scaleChange = new Vector3(0.2f, 0.2f, 0.2f);

            _onBoard = false;
        }

        public void AddClue(string clue)
        {
            _clue = ClueManager.GetClueFromID(clue);
            _image.sprite = _clue.Icon;
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
            OnPlacedClueboard();
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (_mouseDown)
            {
                GameObject image = _image.gameObject;
                // Increases and decreases size of sprite
                var w = Input.mouseScrollDelta.y;
                if (w > 0 && _sizeMax > image.transform.localScale.x) {
                    image.transform.localScale += _scaleChange;
                } else if (w < 0 && _sizeMin < image.transform.localScale.x) {
                    image.transform.localScale -= _scaleChange;
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

        public void OnPlacedClueboard()
        {
            if (_saveClue == null)
            {
                _saveClue = new ClueBoardClue();
            }
            _saveClue.ClueID = Clue.ClueID;
            _saveClue.Position = transform.localPosition;
            _saveClue.Scale = transform.localScale.x;

            _onBoard = true;
            ClueBoardManager.Instance.RemoveFromBin(this);
        }
    }
}
