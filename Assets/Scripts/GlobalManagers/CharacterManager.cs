using System;
using System.Collections.Generic;
using UnityEngine;
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

    public static CharacterState GetCharacterStateFromID(string charID)
    {
        CharacterState cState = GameManager.State.CharacterStates.Find(state => state.CharacterID == charID);
        if (cState == null)
        {
            throw new NullReferenceException($"Character State of {charID} could not be found. Check spelling.");
        }

        return cState;
    }

    [YarnCommand("set_character_node")]
    public static void SetCurrentNode(string charID, string currNode = "")
    {
        CharacterState cState = GetCharacterStateFromID(charID);
        cState.CurrentNode = currNode;
    }
}