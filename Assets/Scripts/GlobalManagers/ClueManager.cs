using UnityEngine;

public class ClueManager : MonoBehaviour
{
    [SerializeField] private ClueDatabase m_clueDatabase;
    [SerializeField] private ClueCombinationDatabase m_deductionDatabase;
    public ClueDatabase ClueDatabase => m_clueDatabase;
    public ClueCombinationDatabase DeductionDatabase => m_deductionDatabase;

    public void Awake()
    {
        ClueDatabase.GenerateDictionary();
        DeductionDatabase.GenerateDictionary();
    }

    public static Clue GetClueFromID(string clueID)
    {
        return GameManager.ClueManager.ClueDatabase.Clues[clueID];
    }

    public static Clue GetClueFromDeduction(string clueID1, string clueID2)
    {
        Hash128 hash = new Hash128();
        hash.Append(clueID1);
        hash.Append(clueID2);

        return GameManager.ClueManager.DeductionDatabase.DeductDict[hash];
    }
}
