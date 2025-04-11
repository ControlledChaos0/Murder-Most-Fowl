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
    public string ConvoChar = "";
    public List<CharacterState> CharacterStates;

    [Header("Dialogue State")]
    public bool Tutorial = true;
    public int CrowRelationship = 0;

    [Header("Inventory")] 
    public List<string> DiscoveredClues = new();

    [Header("Clueboard Inventory")]
    public List<ClueBoardClue> ClueBoardClues = new();
    public List<ClueBoardNote> ClueBoardNotes = new();
    public List<string> NewClueBin = new();
    public List<string> ArchivedClueBin = new();
    
}

[CreateAssetMenu(fileName = "New State Asset", menuName = "Scriptable Objects/State Asset")]
public class StateAsset : ScriptableObject
{
    [SerializeField]
    State m_state;
    public State State => m_state;
}



