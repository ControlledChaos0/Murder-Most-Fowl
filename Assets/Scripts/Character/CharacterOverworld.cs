using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class CharacterOverworld : MonoBehaviour,
    IPointerClickHandler
{
    [SerializeField] private string characterID;
    [SerializeField] private string yarnNode => GetCurrentNode();
    public void OnPointerClick(PointerEventData eventData)
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
        return CharacterManager.GetCharacterStateFromID(characterID).CurrentNode;
    }

    public string GetCurrentDismissal()
    {
        return CharacterManager.GetCharacterStateFromID(characterID).CurrentDismissal;
    }
}
