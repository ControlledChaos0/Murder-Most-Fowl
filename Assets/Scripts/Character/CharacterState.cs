using UnityEngine;
using MessagePack;
using System.Collections.Generic;

[MessagePackObject(AllowPrivate = true), System.Serializable]
public partial class CharacterState
{

    [Key(0), SerializeField]
    private string m_name;

    [IgnoreMember]
    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }

    [Key(1), SerializeField]
    private string m_currNode;

    [IgnoreMember]
    public string CurrentNode
    {
        get { return m_currNode; }
        set { m_currNode = value; }
    }

    [Key(2), SerializeField]
    private string m_currDismissal;

    [IgnoreMember]
    public string CurrentDismissal
    {
        get { return m_currDismissal; }
        set { m_currDismissal = value; }
    }
}