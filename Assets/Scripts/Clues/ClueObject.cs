using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

namespace Clues
{
    public class ClueObject : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField] private string _clueID;
        [SerializeField] private string _node;
        [SerializeField] private bool _disappearOnClick = true;
        [SerializeField] private bool _disabledInTutorial = false;

        private SpriteRenderer _spriteRenderer;
        private bool _hovered;
        private bool _found;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // TODO
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_disabledInTutorial)
            {
                _spriteRenderer.enabled = false;
            }
        }

        private void Update()
        {
            if (!_disabledInTutorial || !GameManager.StateManager.ActiveState.Tutorial)
            {
                _spriteRenderer.enabled = true;
            }
        }

        private void OnEnable()
        {
            CoroutineUtils.ExecuteAfterEndOfFrame(EnableActions, this);
        }

        private void OnDisable()
        {
            if (_hovered)
            {
                CursorManager.Instance.SetToMode(ModeOfCursor.Default);
                _hovered = false;
            }
            // TODO
            // CameraController.Instance.PointerEnterAction -= OnPointerEnter;
            // CameraController.Instance.PointerExitAction -= OnPointerExit;
        }

        private void EnableActions()
        {
            // TODO
            // CameraController.Instance.PointerEnterAction += OnPointerEnter;
            // CameraController.Instance.PointerExitAction += OnPointerExit;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // TODO
            Debug.Log("Pointer Enter!");
            CursorManager.Instance.SetToMode(ModeOfCursor.Inspect);
            _hovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // TODO
            Debug.Log("Pointer Exit!");
            CursorManager.Instance.SetToMode(ModeOfCursor.Default);
            _hovered = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // TODO
            Debug.Log("Click the clue!");
            if (!_disabledInTutorial || !GameManager.StateManager.ActiveState.Tutorial)
            {
                if (!_found)
                {
                    _found = true;
                    ClueBoardManager.Instance.InstantiateClue(_clueID);
                    DialogueHelper.Instance.DialogueRunner.StartDialogue(_node);
                }
                if (_disappearOnClick)
                {
                    Collect();
                }
            }
            // if (!_found)
            // {
            //     _found = true;
            //     ClueBoardManager.Instance.InstantiateClue(_clueID);
            //     DialogueHelper.Instance.DialogueRunner.StartDialogue(_node);
            // }
            // if (_disappearOnClick)
            // {
            //     Collect();
            // }
        }

        [YarnCommand("collect_clue")]
        public void Collect()
        {
            gameObject.SetActive(false);
        }
    }
}
