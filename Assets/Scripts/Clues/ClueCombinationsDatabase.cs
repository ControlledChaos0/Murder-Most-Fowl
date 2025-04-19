using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Clue Combination Database", menuName = "Scriptable Objects/Clues/Clue Combination Database")]
public class ClueCombinationDatabase : ScriptableObject
{
    [SerializeField]
    private Deduction[] deductions;

    public Dictionary<Hash128, Clue> DeductDict { get; private set; } = new();

    private void OnValidate()
    {
        if (DeductDict == null) DeductDict = new();
        DeductDict.Clear();

        int i = 0;
        foreach (Deduction c in deductions)
        {
            if (c == null || !c.clue1 || !c.clue2 || !c.deduction) continue;

            Hash128 hash1 = new();
            hash1.Append(c.clue1.ClueID);
            hash1.Append(c.clue2.ClueID);
            Hash128 hash2 = new();
            hash2.Append(c.clue2.ClueID);
            hash2.Append(c.clue1.ClueID);

            if (DeductDict.ContainsKey(hash1) || DeductDict.ContainsKey(hash2))
            {
                Debug.LogError($"The deduction at index {i} with the same clues, \"{c.clue1.ClueID}\" and \"{c.clue2.ClueID}\", has already been added to the Database.");
                continue;
            }

            DeductDict.Add(hash1, c.deduction);
            DeductDict.Add(hash2, c.deduction);
            i++;
        }
    }

    public void GenerateDictionary()
    {
        OnValidate();
    }
}

[Serializable]
public class Deduction
{
    public Clue clue1;
    public Clue clue2;
    public Clue deduction;
}