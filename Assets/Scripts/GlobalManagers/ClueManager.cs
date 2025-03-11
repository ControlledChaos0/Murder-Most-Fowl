using GlobalManagers;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    [SerializeField]
    private ClueDatabase m_clueDatabase;
    public ClueDatabase ClueDatabase => m_clueDatabase;

    public static Clue GetClueFromID(string clueID)
    {
        return GameManager.ClueManager.ClueDatabase.Clues[clueID];
    }
}
