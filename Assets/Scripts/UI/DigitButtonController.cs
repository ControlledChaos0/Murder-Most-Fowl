using TMPro;
using UnityEngine;

namespace UI
{
    public class CounterController : MonoBehaviour
    {
        // The current value of the counter, which will be displayed
        public int counterValue;
        
        // Reference to the NumberSelector script to check if the combo is solved
        private NumberSelector _numberSelector;

        private void Awake()
        {
            // Get the reference to NumberSelector script in the scene
            _numberSelector = FindFirstObjectByType<NumberSelector>();
        }
        
        // Method to increase or decrease the counter value
        private void ChangeCounterValue(int delta)
        {
            // Modify counterValue by delta (1 for increase, -1 for decrease)
            counterValue = (counterValue + delta + 10) % 10;
            
            // Update the displayed value of the counter to the new counterValue
            gameObject.GetComponent<TMP_Text>().text = counterValue.ToString();
        
            // After changing the counter, check if the combo is solved
            _numberSelector.CheckIfSolved();
        }

        public void Increase()
        {
            // Increase the counter by 1
            ChangeCounterValue(1);
        }

        public void Decrease()
        {
            // Decrease the counter by 1
            ChangeCounterValue(-1);
        }
    }
}