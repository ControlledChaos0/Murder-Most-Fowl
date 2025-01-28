using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

namespace Clues
{
    public class ClueObject : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField] private GameObject clueObjectUI;
        [SerializeField] private bool disappearOnClick = true;

        private SpriteRenderer _spriteRenderer;
        private bool _found;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // TODO
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            CoroutineUtils.ExecuteAfterEndOfFrame(EnableActions, this);
        }

        private void OnDisable()
        {
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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // TODO
            Debug.Log("Pointer Exit!");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // TODO
            Debug.Log("Click the clue!");
            if (!_found)
            {
                Instantiate(clueObjectUI);
            }
            if (disappearOnClick)
            {
                Collect();
            }
        }

        [YarnCommand("collect_clue")]
        public void Collect()
        {
            gameObject.SetActive(false);
        }
    }
}
