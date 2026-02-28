using Manager;
using UnityEngine;

public class Safe : PlayerInteractable
{
    [SerializeField] private string _combo;
    [SerializeField] private string _solvedDialogue;
    [SerializeField]
    private bool _open = false;
    private bool _firstSolve = false;

    public string Combo
    {
        get => _combo;
    }

    public void Start()
    {
        _firstSolve = _open;
    }

    protected override void OnPointerClick()
    {
        //CommandManager.Instance.Queue(new DialogueCommand("GanderSafe"));
        CommandManager.Instance.Queue(new SafeCommand(this));
    }

    public void OnSolved()
    {
        _open = true;
        PuzzleSafe.Instance.DisablePuzzleScreen();
        SolveInternal();
    }

    public void Solve()
    {
        if (_open)
        {
            SolveInternal();
        }
        else
        {
            PuzzleSafe.Instance.ActivatePuzzleScreen(this);
        }
    }

    private void SolveInternal()
    {
        if (!_firstSolve)
        {
            DialogueHelper.Instance.DialogueRunner.StartDialogue(_solvedDialogue);
            _firstSolve = true;
        } else
        {
            DialogueHelper.Instance.DialogueRunner.StartDialogue("OpenSafe");
        }
    }

}

public class SafeCommand : PlayerInteractableCommand
{
    protected Safe Safe
    {
        get => m_Interactable as Safe;
    }
    public SafeCommand(PlayerInteractable interactable) : base(interactable)
    {
    }

    protected override void ExecuteCommand()
    {
        Safe.Solve();
    }

    protected override bool IsCompletedInternal()
    {
        return true;
    }
}