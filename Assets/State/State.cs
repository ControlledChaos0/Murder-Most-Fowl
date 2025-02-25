using UnityEngine;
using MessagePack;

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
}



