using UnityEngine;
using MessagePack;
using System.Collections.Generic;

[MessagePackObject(AllowPrivate = true), System.Serializable]
public partial class ClueBoardClue
{
    [Key(0), SerializeField]
    private string m_clueID;
    [IgnoreMember]
    public string ClueID => m_clueID;

    [IgnoreMember]
    public Clue Clue => ClueManager.GetClueFromID(m_clueID);

    [Key(1), SerializeField]
    private float m_x;
    [Key(2), SerializeField]
    private float m_y;

    [IgnoreMember]
    public Vector2 Position
    {
        get { return new Vector2(m_x, m_y); }
        set
        {
            m_x = value.x;
            m_y = value.y;
        }
    }

    [Key(3), SerializeField]
    private float m_scale;
    [IgnoreMember]
    public float Scale => m_scale;

    [Key(4), SerializeReference]
    private List<ClueBoardClue> m_connectedClues;
    [IgnoreMember]
    public List<ClueBoardClue> ConnectedClues => m_connectedClues;
}
