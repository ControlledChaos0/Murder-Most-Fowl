using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Clue Database", menuName = "Scriptable Objects/Clue Database")]
public class ClueDatabase : ScriptableObject
{
    [SerializeField]
    private Clue[] m_clues;

    public Dictionary<string, Clue> Clues { get; private set; } = new();

    private void OnValidate()
    {
        if (Clues == null) Clues = new();
        Clues.Clear();

        foreach (Clue c in m_clues)
        {
            if (c == null) continue;
            if (Clues.ContainsKey(c.ClueID))
            {
                Debug.LogWarning($"A clue with the same ID, \"{c.ClueID},\" has already been added to the Clue Database.");
                continue;
            }

            Clues.Add(c.ClueID, c);
        }
    }
}
