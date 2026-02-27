using UnityEngine;

public class DialogueCommand : PlayerInteractableCommand
{
    protected CharacterOverworld Character
    {
        get => m_Interactable as CharacterOverworld;
    }

    public DialogueCommand(CharacterOverworld character) : base(character) { }

    public override void Stop()
    {
        return;
    }

    protected override void ExecuteCommand()
    {
        Character.StartDialogue();
    }

    protected override bool IsCompletedInternal()
    {
        return true;
    }
}
