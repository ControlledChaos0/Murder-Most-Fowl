using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Yarn.Unity;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private CharacterDatabase m_characterDatabase;
    public CharacterDatabase CharacterDatabase => m_characterDatabase;

    public List<CharacterState> InitCharacterState(State activeState)
    {
        List<CharacterState> charStates = new();
        foreach (Character c in CharacterDatabase.CharacterList)
        {
            CharacterState cState = new();
            cState.CharacterID = c.CharacterID;
            cState.Name = c.Name;
            cState.CurrentNode = c.StartingNode;
            cState.VisitedCurrNode = false;
            cState.CurrentIdle = c.StartingIdle;
            cState.CurrentDismissal = c.StartingDismissal;
            cState.CharacterClues = new();

            foreach (Character.ClueResponse clueResponse in c.ClueResponses)
            {
                CharacterClue cClue = new CharacterClue()
                {
                    ClueID = clueResponse.clueID,
                    NodeResponse = clueResponse.nodeResponse,
                    ShownClue = false,
                };

                cState.CharacterClues.Add(cClue);
            }

            charStates.Add(cState);
        }

        return charStates;
    }

    public CharacterState GetCharacterStateFromID(string charID)
    {
        CharacterState cState = GameManager.State.CharacterStates.Find(state => state.CharacterID == charID);
        if (cState == null)
        {
            throw new NullReferenceException($"Character State of {charID} could not be found. Check spelling.");
        }

        return cState;
    }

    public string GetClueResponse(string clueID)
    {
        string charID = GameManager.State.ConvoChar;
        return GetClueResponse(charID, clueID);
    }

    public string GetClueResponse(string characterID, string clueID)
    {
        if (GetCharacterStateFromID(characterID).CharClueDict.TryGetValue(clueID, out CharacterClue cClue))
        {
            if (cClue.ShownClue && characterID != "Penguin")
            {
                return GetCurrentDismissal(characterID);
            }
            cClue.ShownClue = true;
            return cClue.NodeResponse;
        }

        return GetCurrentDismissal(characterID);
    }

    public string GetCurrentNode(string characterID)
    {
        CharacterState cState = GetCharacterStateFromID(characterID);
        if (cState.VisitedCurrNode)
        {
            return cState.CurrentIdle;
        }

        cState.VisitedCurrNode = true;
        return cState.CurrentNode;
    }

    public string GetCurrentDismissal(string characterID)
    {
        return GetCharacterStateFromID(characterID).CurrentDismissal;
    }

    [YarnCommand("set_character_node")]
    public static void SetCurrentNode(string charID, string currNode = "")
    {
        CharacterState cState = GameManager.CharacterManager.GetCharacterStateFromID(charID);
        cState.CurrentNode = currNode;
    }

    [YarnCommand("set_character_idle_node")]
    public static void SetIdleNode(string charID, string idleNode = "")
    {
        CharacterState cState = GameManager.CharacterManager.GetCharacterStateFromID(charID);
        cState.CurrentNode = idleNode;
    }

    public void ClearConvoChar()
    {
        GameManager.State.ConvoChar = "";
    }

    [YarnCommand("ClearClue")]
    public static void ClearClue(string charID, string clueID)
    {
        CharacterState cState = GameManager.CharacterManager.GetCharacterStateFromID(charID);
        CharacterClue charClue = cState.CharClueDict[clueID];
        charClue.ShownClue = false;
    }
}