using UnityEngine;

public class ToggleCommand : PlayerInteractableCommand
{
    protected ToggleInteractable Toggleable
    {
        get => m_Interactable as ToggleInteractable;
    }

    public ToggleCommand(ToggleInteractable toggleable) : base(toggleable) { }

    protected override void ExecuteCommand()
    {
        Toggleable.Toggle();
    }

    protected override bool IsCompletedInternal()
    {
        return true;
    }
}
