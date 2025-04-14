using UnityEngine;

public class ShelfInteractToggle : MonoBehaviour
{
    [SerializeField] 
    private GameObject _shelf;
    private ShelfController _shelfController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shelfController = _shelf.GetComponent<ShelfController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (_shelfController.isOpen())
        {
            DialogueHelper.Instance.DialogueRunner.StartDialogue("MissingBottle");
        }
    }
}
