using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.Combat
{
    //This is a quick script to act a a quick AOE, better one should be made later
    public class WeaponB_DeathBurst : WeaponObject
    {
        [SerializeField] private float exsplosiveForce = 5;
        [HideInInspector] public float forceMult = 1;

        public void SetDamage(float NewDamage)
        {
            activeDamage = NewDamage;
        }
        public void SetBaseForce(float NewForce)
        {
            exsplosiveForce = NewForce;
        }

        public override void ApplyDamage(Core.Health Target)
        {
            Target.Damage((int)activeDamage);
            if (Target.GetComponent<Rigidbody2D>())
            {
                Target.GetComponent<Rigidbody2D>().AddForceAtPosition((Target.transform.position - transform.position) * (exsplosiveForce * forceMult), transform.position, ForceMode2D.Impulse);

            }
        }
    }
}