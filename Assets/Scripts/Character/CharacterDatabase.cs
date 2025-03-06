using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Character Database", menuName = "Scriptable Objects/Characters/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField]
    private Character m_goose;
    [SerializeField]
    private Character m_peacock;
    [SerializeField]
    private Character m_crow;
    [SerializeField]
    private Character m_penguin;

    public List<Character> CharacterList;

    public Character Goose => m_goose;
    public Character Peacock => m_peacock;
    public Character Crow => m_crow;
    public Character Penguin => m_penguin;

    private void OnValidate()
    {
        if (CharacterList == null) CharacterList = new();
        CharacterList.Clear();

        if (m_goose != null)
        {
            CharacterList.Add(m_goose);
            if (m_goose.CharacterID != "Goose")
            {
                Debug.LogWarning($"Goose has the wrong ID of \"{m_goose.CharacterID},\", which could indicate an incorrect character has been placed.");
            }
        }
        if (m_peacock != null)
        {
            CharacterList.Add(m_peacock);
            if (m_peacock.CharacterID != "Peacock")
            {
                Debug.LogWarning($"Peacock has the wrong ID of \"{m_peacock.CharacterID},\", which could indicate an incorrect character has been placed.");
            }
        }
        if (m_crow != null)
        {
            CharacterList.Add(m_crow);
            if (m_crow.CharacterID != "Crow")
            {
                Debug.LogWarning($"Crow has the wrong ID of \"{m_crow.CharacterID},\", which could indicate an incorrect character has been placed.");
            }
        }
        if (m_penguin != null)
        {
            CharacterList.Add(m_penguin);
            if (m_penguin.CharacterID != "Penguin")
            {
                Debug.LogWarning($"Penguin has the wrong ID of \"{m_penguin.CharacterID},\", which could indicate an incorrect character has been placed.");
            }
        }
    }
}