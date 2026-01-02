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

    //[Header("Player")]
    //public PlayerState PlayerState;

    [Header("Characters")]
    public string ConvoChar = "";
    public List<CharacterState> CharacterStates;

    [Header("Dialogue State")]
    public bool Tutorial = true;
    public int CrowRelationship = 0;
    public bool HasShards = false;
    public bool PresentedShards = false;
    public bool DuckBroke = false;
    public bool PeacockArgument = false;
    public bool DeliverLetter = false;
    public bool PresentedReed = false;
    public bool FixedRelationship = false;
    public bool MorganaTestimony = false;
    public bool ShowedGloves = false;
    public bool CrowGlovesStory = false;
    public bool HasLetter = false;
    public bool NoticedManifest = false;
    public bool CrowCigs = false;
    public bool CrowDone = false;
    public bool GanderBriefcase = false;
    public bool CockfightingArena = false;
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



