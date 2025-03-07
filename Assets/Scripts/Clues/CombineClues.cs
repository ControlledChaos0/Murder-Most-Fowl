using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.SerializableDictionary;

namespace  Clues
{
    public class CombineClues : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<string, GameObject> clueCombinations;
        [SerializeField] private ClueBoardBin ClueBin;
        
        /* Will need rework when UI is created, this is just a hard check on player input, but The logic after checking
         should remain the same so that the result is properly added
         
        Dictionary maps input to a resultant clue prefab, where the prefab is then instantiated,
        and the ClueObjectUI is set up so that it can appear in the bin, by first getting the ClueObjectUI, and then 
        getting the clue using the Prefab's name so that it is properly created, then added to the ClueBoardBin */
        public void Combine(string inputText)
        {
            if (clueCombinations.TryGetValue(inputText, out var resultCluePrefab))
            {
                GameObject newClue = Instantiate(resultCluePrefab);
                newClue.TryGetComponent<ClueObjectUI>(out ClueObjectUI clueObjectUI);
                if (!clueObjectUI) return;
                clueObjectUI.AddClue(resultCluePrefab.name);
                Debug.Log(clueObjectUI.Clue);
                ClueBin.AddToBin(clueObjectUI);
            }
            else
            {
                Debug.Log("Invalid Combination!");
            }
        }
    }   
}