using UnityEngine;

namespace Controller
{
    public class CabinetController : MonoBehaviour
    {
        [SerializeField] private Sprite openSprite;
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private BoxCollider closedCollider;
        [SerializeField] private BoxCollider openCollider1;
        [SerializeField] private BoxCollider openCollider2;
        private bool _open;
        private SpriteRenderer _sr;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
            _sr.sprite = closedSprite;
            openCollider1.enabled = false;
            openCollider2.enabled = false;
            _open = false;
        }

        private void OnMouseDown()
        {
            if (_open)
            {
                _sr.sprite = closedSprite;
                openCollider1.enabled = false;
                openCollider2.enabled = false;
                closedCollider.enabled = true;
                _open = false;
            } else {
                _sr.sprite = openSprite;
                openCollider1.enabled = true;
                openCollider2.enabled = true;
                closedCollider.enabled = false;
                _open = true;
            }
        }

        public bool IsOpen() 
        {
            return _open;
        }
    }
}
