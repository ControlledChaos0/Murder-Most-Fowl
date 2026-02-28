using UnityEngine;

public class CharacterDialogueCommand : PlayerInteractableCommand
{
    protected CharacterOverworld Character
    {
        get => m_Interactable as CharacterOverworld;
    }

    public CharacterDialogueCommand(CharacterOverworld character) : base(character) { }

    protected override void StopCommand()
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

public class DialogueCommand : Command
{
    private string m_Node;
    public DialogueCommand(string node)
    {
        m_Node = node;
    }

    protected override void StopCommand()
    {
        return;
    }

    protected override void ExecuteCommand()
    {
        DialogueHelper.Instance.DialogueRunner.StartDialogue(m_Node);
    }

    protected override bool IsCompletedInternal()
    {
        //bool run = DialogueHelper.Instance.DialogueRunner.IsDialogueRunning;
        //if (run)
        //{
        //    Debug.Log("DIALOGUE COMMAND FINISHED");
        //}
        //return DialogueHelper.Instance.DialogueRunner.IsDialogueRunning;
        return true;
    }

    protected override void ReadyCommand()
    {
        return;
    }
}
