using UnityEngine;
using MessagePack;
using System.Collections.Generic;

[MessagePackObject(AllowPrivate = true), System.Serializable]
public partial class CharacterState
{
    [Key(0), SerializeField]
    private string m_charID;

    [IgnoreMember]
    public string CharacterID
    {
        get { return m_charID; }
        set { m_charID = value; }
    }

    [Key(1), SerializeField]
    private string m_name;

    [IgnoreMember]
    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }

    [Key(2), SerializeField]
    private string m_currNode;

    [IgnoreMember]
    public string CurrentNode
    {
        get { return m_currNode; }
        set
        {
            m_visitedNode = false;
            m_currNode = value;
        }
    }

    [Key(3), SerializeField] 
    private bool m_visitedNode;
    [IgnoreMember]
    public bool VisitedCurrNode
    {
        get { return m_visitedNode; }
        set { m_visitedNode = value; }
    }

    [Key(4), SerializeField]
    private string m_currIdle;

    [IgnoreMember]
    public string CurrentIdle
    {
        get { return m_currIdle; }
        set { m_currIdle = value; }
    }


    [Key(5), SerializeField]
    private string m_currDismissal;

    [IgnoreMember]
    public string CurrentDismissal
    {
        get { return m_currDismissal; }
        set { m_currDismissal = value; }
    }

    [Key(6), SerializeReference]
    private List<CharacterClue> m_characterClues;

    [IgnoreMember]
    public List<CharacterClue> CharacterClues
    {
        get { return m_characterClues; }
        set { m_characterClues = value; }
    }

    [IgnoreMember] 
    private Dictionary<string, CharacterClue> m_charClueDict;

    [IgnoreMember]
    public Dictionary<string, CharacterClue> CharClueDict
    {
        get
        {
            if (m_charClueDict == null)
            {
                m_charClueDict = new();
                foreach (CharacterClue charClue in m_characterClues)
                {
                    m_charClueDict[charClue.ClueID] = charClue;
                }
            }

            return m_charClueDict;
        }
        private set
        {
            m_charClueDict = value;
        }
    }

    [Key(7), SerializeField]
    private Vector3 m_position;

    [IgnoreMember]
    public Vector3 Position
    {
        get { return m_position; }
        set { m_position = value; }
    }
}

[MessagePackObject(AllowPrivate = true), System.Serializable]
public partial class CharacterClue
{
    [Key(0), SerializeField]
    private string m_clueID;

    [IgnoreMember]
    public string ClueID
    {
        get { return m_clueID; }
        set { m_clueID = value; }
    }

    [Key(1), SerializeField]
    private string m_nodeResponse;

    [IgnoreMember]
    public string NodeResponse
    {
        get { return m_nodeResponse; }
        set { m_nodeResponse = value; }
    }

    [Key(2), SerializeField]
    private bool m_shownClue;

    [IgnoreMember]
    public bool ShownClue
    {
        get { return m_shownClue; }
        set { m_shownClue = value; }
    }
}