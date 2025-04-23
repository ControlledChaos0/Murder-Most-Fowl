using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Clues
{
    public class ClueObjectUI : MonoBehaviour, 
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IScrollHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Pin _pin;
        public Image Image => _image;
        public GameObject _menu;

        private Clue _clue;
        private ClueBoardClue _saveClue;

        private bool _onBoard;
        private bool _inBin;

        private Vector2 _offset;
        private Vector2 _menuOffset;
        private Vector3 _worldOffset;
        private bool _mouseDown;
        private bool _dragging;
        private bool _scaling;

        private static readonly float _sizeMin = .5f;
        private static readonly float _sizeMax = 3.0f;
        private Vector3 _initialScale;
        private Vector3 _scaleChange;

        public Clue Clue
        {
            get => _clue;
            private set => _clue = value;
        }

        // Start is called before the first frame update
        private void Start()
        {
            OnPlacedBin(ClueBoardManager.Instance.NewBin);
            _mouseDown = false;
            _scaleChange = new Vector3(0.2f, 0.2f, 0.2f);

            _onBoard = false;
            _scaling = false;
            _dragging = false;
            _initialScale = Vector3.one;
        }

        public void AddClue(string clueID)
        {
            _clue = ClueManager.GetClueFromID(clueID);
            _image.sprite = _clue.Icon;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector2 mousePos = eventData.pressPosition;
            Vector2 uiPos = transform.position;
            _offset = uiPos - mousePos;

            transform.parent = ClueBoardManager.Instance.Front;
            Vector3 mousePosWorld = Camera.main.WorldToScreenPoint(new Vector3(mousePos.x, mousePos.y, 0));
            Vector3 uiPosWorld = Camera.main.WorldToScreenPoint(new Vector3(uiPos.x, uiPos.y, 0));
            _worldOffset = uiPosWorld - mousePosWorld;

            GameObject image = _image.gameObject;
            Vector2 menuPos = _menu.transform.position;
            Vector2 cornerPos = image.transform.Find("Corner").transform.position;
            _menuOffset = cornerPos - menuPos;

            var renderer = _image.GetComponent<RectTransform>();
            var width = renderer.rect.width * _image.transform.localScale.x;
            var height = renderer.rect.height * _image.transform.localScale.y;
            var margin = 5;

            if (!_inBin && (_offset.x > (width / 2 - margin) * 0.6f || _offset.y > (height / 2 - margin) * 0.6f || _offset.x < (-width / 2 + margin) * 0.6f || _offset.y < (-height / 2 + margin) * 0.6f))
            {
                _scaling = true;
                _initialScale = _image.transform.localScale;
            }
            else
            {
                _scaling = false;
            }

            _dragging = true;

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_scaling == true){
                Vector3 mousePosWorld = Camera.main.WorldToScreenPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
                Vector3 uiPosWorld = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));
                Vector3 newOffset = uiPosWorld - mousePosWorld;
                
                var change = newOffset.magnitude/_worldOffset.magnitude;
                if ((_sizeMax > _image.transform.localScale.x) && (newOffset.magnitude > _worldOffset.magnitude)) {
                    // Scale up
                    if (_initialScale.x * change > _sizeMax){
                        // reach maximum size
                        _image.transform.localScale = new Vector3(_sizeMax, _sizeMax, 1);
                    } else {
                        _image.transform.localScale = _initialScale * change; 
                    }
                } else if (_sizeMin < _image.transform.localScale.x && newOffset.magnitude < _worldOffset.magnitude){
                    // Scale Down
                    if (_initialScale.x * change < _sizeMin){
                        // reach minimum size
                        _image.transform.localScale = new Vector3(_sizeMin, _sizeMin, 1);

                    } else {
                        _image.transform.localScale = _initialScale * change; 
                    }
                    
                }

                GameObject image = _image.gameObject;
                Vector2 cornerPos = image.transform.Find("Corner").transform.position;
                _menu.transform.position = cornerPos - _menuOffset;
            } else {
                transform.position = eventData.position + _offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _offset = Vector2.zero;
            List<RaycastResult> results = new List<RaycastResult>();
            ClueBoardManager.Instance.GraphicRaycast.Raycast(eventData, results);
            bool placed = false;
            foreach(RaycastResult result in results)
            {
                if (placed)
                {
                    break;
                }
                switch (result.gameObject.tag)
                {
                    case ("Bin"):
                        OnPlacedBin(result.gameObject.GetComponent<NewBin>());
                        placed = true;
                        break;
                    case ("Board"):
                        OnPlacedClueboard();
                        placed = true;
                        break;
                }
            }

            if (!placed)
            {
                // TODO
                // Fix being able to place the UI anywhere but the folder and the clueboard
            }

            _scaling = false;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (!_onBoard)
            {
                return;
            }

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
                
                Vector2 cornerPos = image.transform.Find("Corner").transform.position;
                _menu.transform.position = cornerPos - _menuOffset;
            }
            else
            {
                ClueBoard.Instance.OnScroll(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _mouseDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _mouseDown = false;

            if (!_dragging) {
                if (eventData.pointerEnter && _pin.gameObject.Equals(eventData.pointerDrag))
                {
                    return;
                }
                if (_onBoard)
                {
                    bool isActive = _menu.activeSelf;
                    //_menu.SetActive(!isActive);
                }
            }
            _dragging = false;
        }

        public void OnPlacedClueboard()
        {
            transform.parent = ClueBoardManager.Instance.Clues;
            if (_saveClue == null)
            {
                _saveClue = new ClueBoardClue();
                _saveClue.ClueID = Clue.ClueID;
            }
            _saveClue.Position = transform.localPosition;
            _saveClue.Scale = _image.transform.localScale.x;

            _onBoard = true;
            if (_inBin)
            {
                ClueBoardManager.Instance.NewBin.RemoveFromBin(this);
                _inBin = false;
                ClueInventoryManager.Instance.AddClueBoardClue(_saveClue);
            }
            else
            {
                ClueInventoryManager.Instance.UpdateClueBoardClue(_saveClue);
            }

            _pin.gameObject.SetActive(true);
        }

        public void OnPlacedBin(NewBin binGO)
        {
            //Clue
            if (_onBoard)
            {
                ClueInventoryManager.Instance.DeleteClueBoardClue(_saveClue);
                _onBoard = false;
            }

            if (!_inBin)
            {
                ClueBoardManager.Instance.NewBin.AddToBin(this);
                _inBin = true;
            }

            _pin.gameObject.SetActive(false);
        }

        public void PresentEvidence()
        {
            _menu.SetActive(false);
            ClueBoardManager.Instance.PresentEvidence(Clue);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!ClueBoardManager.Instance.SelectedClue)
            {
                SetDisplay();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!ClueBoardManager.Instance.SelectedClue)
            {
                ClueBoardManager.Instance.ChangeDisplay();
            }
        }

        private void SetDisplay()
        {
            ClueBoardManager.Instance.ChangeDisplay(_clue.ClueID);
        }

        public void OnSelect(BaseEventData eventData)
        {
            SetDisplay();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            ClueBoardManager.Instance.SelectedClue._menu.SetActive(false);
            ClueBoardManager.Instance.ChangeDisplay();
            ClueBoardManager.Instance.SelectedClue = null;
        }
    }
}
