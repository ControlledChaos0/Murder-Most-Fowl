using MessagePack;
using UnityEngine;

[MessagePackObject(AllowPrivate = true), System.Serializable]
public partial class PlayerState
{
    [Key(0), SerializeField]
    private PlayerAction m_charID;

    [IgnoreMember]
    public PlayerAction CharacterID
    {
        get { return m_charID; }
        set { m_charID = value; }
    }
}

public enum PlayerAction
{
    Default,
    Overworld,
    Dialogue,
    Board
}