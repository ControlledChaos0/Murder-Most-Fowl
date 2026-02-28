using System;
using UnityEngine;

public class ActionCommand : PlayerInteractableCommand
{
    private Action _action;
    public ActionCommand(Action action, PlayerInteractable interactable) : base(interactable)
    {
        _action = action;
    }
    protected override void ExecuteCommand()
    {
        _action?.Invoke();
    }

    protected override bool IsCompletedInternal()
    {
        return true;
    }
}
