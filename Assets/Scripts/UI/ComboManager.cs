using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class NumberSelector : MonoBehaviour
    {
        // A string of digits (0-9) representing the combination to build the counters
        [SerializeField] private string _combo;
        
        // Gap between counters in UI
        [SerializeField] private float gap = 200f;
        
        // Whether to update the counters automatically when combo or gap changes
        [SerializeField] private bool updateCountersAutomatically;
        
        // Prefab for the counter (displays each digit of the combo)
        [SerializeField] private GameObject comboCounterTemplate;
        
        // Container to hold all the instantiated combo counters
        [SerializeField] private GameObject comboCountersContainer;
        
        // To keep track of the previous combo and gap to prevent unnecessary updates
        private string _previousCombo;
        private float _previousGap = 200f;
        private bool _isSolved = false;

        public event Action Solve;

        // Called when changes are made in the Inspector to validate and update counters
        private void OnValidate()
        {
            // Only update counters if enabled, and if the combo string has changed
            if (!updateCountersAutomatically || (_combo == _previousCombo && (int) gap == (int) _previousGap)) return;
            var filteredCombo = new string(_combo.Where(char.IsDigit).ToArray());
            UpdateComboCounters(filteredCombo);
            
            // Store current combo, gap to compare later
            _previousCombo = _combo;
            _previousGap = gap;
        }

        private void Start()
        {
            _isSolved = false;
        }

        // Method to check if all counters match the specified combo
        public void CheckIfSolved()
        {
            if (_isSolved) return;
            // Get all the combo counter controllers in the container
            var counters = comboCountersContainer.GetComponentsInChildren<CounterController>();

            // Flag to track if the combo has been solved
            var isSolved = true;
            
            for (int i = 0; i < counters.Length; i++)
            {
                // Get the number from the counter
                var counterValue = counters[i].counterValue;

                // Compare it with the corresponding digit in the combo
                if (counterValue != (_combo[i] - '0'))
                {
                    isSolved = false;
                    break;
                }
            }

            _isSolved |= isSolved;
            if ( _isSolved)
            {
                Solve?.Invoke();
            }

            // Log success if the combo is solved, otherwise failure
            Debug.Log(isSolved ? "Success: Combo matched!" : "The combo doesn't match.");
        }

        // Method to update the combo counters based on the current combo string
        private void UpdateComboCounters(string filteredCombo)
        {
            // Destroy all previously instantiated combo counter objects
            foreach (Transform child in comboCountersContainer.transform)
            {
                #if UNITY_EDITOR
                // In Unity Editor, delay the destruction of game objects to avoid errors
                UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);
                #endif
            }
            
            // Starting position for the first counter
            var currentOffsetX = 0f;
            
            // Iterate through each character in the combo string that is a digit
            foreach (var c in _combo.Where(char.IsDigit))
            {
                // Instantiate new counter object for each valid digit
                var counter = Instantiate(comboCounterTemplate, comboCountersContainer.transform.position, Quaternion.identity, comboCountersContainer.transform);
                counter.name = "Counter (" + c + ")";
                
                // Set the counter's RectTransform to position in UI
                var counterRect = counter.GetComponent<RectTransform>();
                counterRect.anchoredPosition = new Vector2(currentOffsetX, counterRect.anchoredPosition.y);
        
                // Adjust the X position for the next counter based on the gap
                currentOffsetX += gap;
            }

            // Update the combo string to only include valid digits
            _combo = filteredCombo;
        }

        public void SetCombo(string combo)
        {
            if (combo != _combo)
            {
                var counters = comboCountersContainer.GetComponentsInChildren<CounterController>();
                foreach (var c in counters)
                {
                    c.ChangeCounterValue(0);
                }
                switch (combo.Length)
                {
                    case 4: _previousCombo = "0000"; break;
                    case 6: _previousCombo = "000000"; break;
                    default: Debug.LogError("Shit's still fucked."); break;
                }
                _isSolved = false;
                _combo = combo;
            }
        }
    }
}
