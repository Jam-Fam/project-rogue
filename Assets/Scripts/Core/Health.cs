
using UnityEngine;
using UnityEngine.Events;

namespace ProjectRogue.Core
{
    public class Health : MonoBehaviour
    {
        // Settings
        [SerializeField] private int startingHealth = 100;

        // State
        [SerializeField] public int currentHealth;

        // Events 
        [SerializeField] private UnityEvent DeathEvent;
        
        void Awake()
        {
            currentHealth = startingHealth;
        }

        public void Damage(int damageAmount)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                DeathEvent.Invoke();
            }
        }
    }
}
