using UnityEngine;

public class DialogueCommand : Command
{
    private CharacterOverworld _character;

    public DialogueCommand(CharacterOverworld character)
    {
        _character = character;
    }

    public override void Stop()
    {
        return;
    }

    protected override void ExecuteCommand()
    {
        _character.StartDialogue();
    }

    protected override bool IsCompleted()
    {
        return true;
    }
}
