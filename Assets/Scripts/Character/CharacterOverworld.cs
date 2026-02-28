using System.Collections;
using System.Collections.Generic;
using FMOD;
using Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class CharacterOverworld : PlayerInteractable,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string characterID;
    [SerializeField] private string yarnNode => GetCurrentNode();

    private bool _hovered = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Inspect);
        _hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetToMode(ModeOfCursor.Default);
        _hovered = false;
    }

    void OnDisable()
    {
        if (_hovered)
        {
            CursorManager.Instance.SetToMode(ModeOfCursor.Default);
            _hovered = false;
        }
    }

    public void StartDialogue()
    {
        string currNode = GetCurrentNode();
        if (currNode == "")
        {
            return;
        }
        DialogueHelper.Instance.DialogueRunner.StartDialogue(currNode);
    }

    public string GetCurrentNode()
    {
        return GameManager.CharacterManager.GetCurrentNode(characterID);
    }

    public string GetCurrentDismissal()
    {
        return GameManager.CharacterManager.GetCurrentDismissal(characterID);
    }

    protected override void OnPointerClick() {
        CommandManager.Instance.Queue(new CharacterDialogueCommand(this));
    }
}
