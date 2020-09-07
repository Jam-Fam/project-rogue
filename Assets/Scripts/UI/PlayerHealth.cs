using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.UI
{
    public class PlayerHealth : MonoBehaviour
    {
        // Components 
        private LinearBar healthDisplay;

        private void Awake()
        {
            healthDisplay = GetComponent<LinearBar>();
        }

        public void OnPlayerHealthChanged(int currentHealth, int maxHealth)
        {
            healthDisplay.Progress = (float)currentHealth / (float)maxHealth;
        }
    }
}
