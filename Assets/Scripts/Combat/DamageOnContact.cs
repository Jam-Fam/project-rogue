using UnityEngine;

using ProjectRogue.Core;

namespace ProjectRogue.Combat
{
    public class DamageOnContact : MonoBehaviour
    {
        // Settings 
        [SerializeField] private int contactDamage = 10;

        void OnTriggerEnter2D(Collider2D collider)
        {
            Health other = collider.gameObject.GetComponent<Health>();

            if (other != null)
            {
                Debug.Log(other.name);
                other.Damage(contactDamage);
            }
        }
    }
}
