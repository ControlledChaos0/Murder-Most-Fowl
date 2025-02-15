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

    [Header("Example Values")]

    public bool TestTag = true;
    public string TestString = "Hello!";
}



