using UnityEngine;
using MessagePack;
using System.Collections.Generic;

[MessagePackObject(keyAsPropertyName:true), System.Serializable]
public class State
{
    [Header("Metadata")]

    public uint SaveID;
    public UDateTime SaveCreated;
    public UDateTime LastSaved;
    public uint TimePlayedSeconds;

    [Header("TestScene State")]

    public bool GooseIntroduced = false;
    public bool NoticedSandwich = false;
    public bool HasHandkerchief = false;
    public bool InspectHandkerchief = false;

    [Header("Characters")] 

    public CharacterState GooseState;
    public CharacterState PenguinState;
    public CharacterState CrowState;
    public CharacterState PeacockState;

    [Header("Inventory")]
    public List<ClueBoardClue> ClueBoardClues;
    public List<ClueBoardNote> ClueBoardNotes;
    public List<string> NewClueBin;
    public List<string> ArchivedClueBin;
    
}

[CreateAssetMenu(fileName = "New State Asset", menuName = "Scriptable Objects/State Asset")]
public class StateAsset : ScriptableObject
{
    [SerializeField]
    State m_state;
    public State State => m_state;
}



