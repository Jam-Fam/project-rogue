using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.Combat
{
    public class WeaponObject : HeldObject
    {
        [Header("Weapon Settings")]
        public bool active;
        [SerializeField] protected float activeDamage = 4;

        //ApplyDamage
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!active)
            {
                return;
            }
            if (other.tag == "Damageable")
            {
                ApplyDamage(other.GetComponent<Core.Health>());
            }
        }
        //When Entring Active Attack mode, apply damange to any objects allready in trigger range
        public virtual void InitialAttack()
        {
            //Get tigger area
            Collider2D[] cols = Physics2D.OverlapBoxAll(transform.TransformPoint(GetComponent<BoxCollider2D>().offset)
                , GetComponent<BoxCollider2D>().size
                , transform.eulerAngles.z);

            foreach (Collider2D c in cols)
            {
                if (c.tag == "Damageable")
                {
                    ApplyDamage(c.GetComponent<Core.Health>());
                }
            }

        }
        public virtual void ApplyDamage(Core.Health Target)
        {
            Target.Damage((int)activeDamage);
        } 

    }
}