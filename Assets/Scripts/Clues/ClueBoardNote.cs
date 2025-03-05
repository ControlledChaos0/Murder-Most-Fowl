using UnityEngine;
using MessagePack;

[MessagePackObject(AllowPrivate = true), System.Serializable]
public partial class ClueBoardNote
{
    [Key(0), SerializeField]
    private string m_text;
    [IgnoreMember]
    public string Text => m_text;

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
}
