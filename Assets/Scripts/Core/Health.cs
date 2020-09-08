
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectRogue.Core
{
    [Serializable] public class ChangedEvent : UnityEvent<int,int> { }

    public class Health : MonoBehaviour
    {
        // Settings
        [SerializeField] private int startingHealth = 100;
        [SerializeField] private float invulnerableDuration = 0f;

        // State
        [SerializeField] public int currentHealth;
        private bool isInvulnerable;

        // Events 
        [SerializeField] private UnityEvent DeathEvent;
        [SerializeField] private ChangedEvent ChangedEvent;
        
        void Awake()
        {
            currentHealth = startingHealth;
        }

        public void Damage(int damageAmount)
        {
            if (isInvulnerable) return;

            currentHealth -= damageAmount;
            ChangedEvent.Invoke(currentHealth, startingHealth);

            if (currentHealth <= 0)
            {
                DeathEvent.Invoke();
            }
            else
            {
                isInvulnerable = true;
                StartCoroutine(TemporaryInvulnerability());
            }
        }

        private IEnumerator TemporaryInvulnerability()
        {
            yield return new WaitForSeconds(invulnerableDuration);
            isInvulnerable = false;
        }
    }
}
