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
        return GameManager.CharacterManager.GetCurrentNode(characterID);
    }

    public string GetCurrentDismissal()
    {
        return GameManager.CharacterManager.GetCurrentDismissal(characterID);
    }
}
