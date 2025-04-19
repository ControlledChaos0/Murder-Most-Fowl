using System;
using System.Collections.Generic;
using TMPro;
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
        public Image Image => _image;
        public GameObject _menu;

        private String _description;
        
        private Clue _clue;
        private ClueBoardClue _saveClue;

        private bool _onBoard;
        private bool _inBin;
        private ClueBoardBin _currentBin;

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
            _currentBin = ClueBoardManager.Instance.NewBin;
            OnPlacedBin(_currentBin.gameObject);
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
            _description = _clue.Description;
            print(_description);
            setDescriptionText(_description);
        }

        private void setDescriptionText(String text)
        {
            TMP_Text[] tmp = _menu.GetComponentsInChildren<TMP_Text>();
            tmp[2].text = text;
            print(tmp[2].text);
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

            if (!_inBin && (_offset.x > (width/2 - margin) * 0.6f || _offset.y > (height/2 - margin) * 0.6f || _offset.x < (-width/2 + margin) * 0.6f || _offset.y < (-height/2 + margin) * 0.6f)) {
                _scaling = true;
                _initialScale = _image.transform.localScale;
            } else {
                _scaling = false;
            }

            _dragging = true;
            
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
                        OnPlacedBin(result.gameObject);
                        placed = true;
                        break;
                    case ("Board"):
                        OnPlacedClueboard();
                        placed = true;
                        break;
                }
            }
            _scaling = false;
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
                
                Vector2 cornerPos = image.transform.Find("Corner").transform.position;
                _menu.transform.position = cornerPos - _menuOffset;
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
            if (!_dragging) {
                bool isActive = _menu.activeSelf;
                _menu.SetActive(!isActive);
            }
            _dragging = false;
            _mouseDown = false;
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
                _currentBin.RemoveFromBin(this);
                _inBin = false;
                ClueInventoryManager.Instance.AddClueBoardClue(_saveClue);
            }
            else
            {
                ClueInventoryManager.Instance.UpdateClueBoardClue(_saveClue);
            }
        }
        public void OnPlacedBin(GameObject binGO)
        {
            //Clue
            if (_onBoard)
            {
                ClueInventoryManager.Instance.DeleteClueBoardClue(_saveClue);
                _onBoard = false;
            } else if (_inBin)
            {
                _currentBin.RemoveFromBin(this);
            }

            _currentBin = binGO.GetComponentInParent<ClueBoardBin>();
            _currentBin.AddToBin(this);
            _inBin = true;
        }

        public void PresentEvidence()
        {
            _menu.SetActive(false);
            ClueBoardManager.Instance.PresentEvidence(Clue);
        }
    }
}
