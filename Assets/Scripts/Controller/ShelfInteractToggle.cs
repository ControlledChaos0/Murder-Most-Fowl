using Manager;
using UnityEngine;

public class ShelfInteractToggle : PlayerInteractable
{
    [SerializeField] 
    private GameObject _shelf;
    private ShelfController _shelfController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shelfController = _shelf.GetComponent<ShelfController>();
    }

    protected override void OnPointerClick()
    {
        CommandManager.Instance.Queue(new ActionCommand(Execute, this));
    }

    protected void Execute()
    {
        if (_shelfController.IsOpen)
        {
            DialogueHelper.Instance.DialogueRunner.StartDialogue("MissingBottle");
        }
    }
}
