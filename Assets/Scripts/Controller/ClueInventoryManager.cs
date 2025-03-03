using System;
using UnityEngine;

public class ClueInventoryManager : Singleton<ClueInventoryManager>
{
    //public event Action<ClueBoardClue> ClueboardClueUpdate;
    void Awake()
    {
        InitializeSingleton();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ClueboardClueUpdate += UpdateClue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateClue(ClueBoardClue cbClue)
    {
        //ClueBoardClue prev_cbClue = GameManager.StateManager.ActiveState.ClueBoardClues.Find(
        //    delegate(ClueBoardClue clue)
        //    {
        //        return clue.ClueID == cbClue.ClueID;
        //    });
        //prev_cbClue.Position = cbClue.Position;
        //prev_cbClue.Scale = cbClue.Scale;
    }
}
