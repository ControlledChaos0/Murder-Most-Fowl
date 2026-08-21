using System;
using Manager;
using UnityEngine;

public class DialogueTrigger : PlayerInteractable
{
    [SerializeField]
    private string m_dialogueNode;

    protected override void OnPointerClick()
    {
        CommandManager.Instance.Queue(new ActionCommand(Execute, this));
    }

    protected void Execute()
    {
        if (m_dialogueNode == null)
        {
            Debug.LogError("Dialogue node not found.");
        } else
        {
            DialogueHelper.Instance.DialogueRunner.StartDialogue(m_dialogueNode);
        }
    }
}
