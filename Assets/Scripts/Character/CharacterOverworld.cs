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
        CharacterState cState = CharacterManager.GetCharacterStateFromID(characterID);
        if (cState.VisitedCurrNode)
        {
            return cState.CurrentIdle;
        }

        cState.VisitedCurrNode = true;
        return cState.CurrentNode;
    }

    public string GetCurrentDismissal()
    {
        return CharacterManager.GetCharacterStateFromID(characterID).CurrentDismissal;
    }

    public string GetClueResponse(string clueID)
    {
        if (CharacterManager.GetCharacterStateFromID(characterID).CharClueDict.TryGetValue(clueID, out CharacterClue cClue))
        {
            if (cClue.ShownClue)
            {
                return GetCurrentDismissal();
            }
            cClue.ShownClue = true;
            return cClue.NodeResponse;
        }

        return GetCurrentDismissal();
    }
}
