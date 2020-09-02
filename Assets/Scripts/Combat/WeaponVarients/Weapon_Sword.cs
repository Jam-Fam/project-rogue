using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.Combat
{
    public class Weapon_Sword : WeaponBase
    {
        [Header("Stab Attack")]
        [SerializeField] private float stabDamage = 6;
        [SerializeField] private float stabKnockBack = 4;
        [SerializeField] private float stabDis = .6f;
        [SerializeField] private float stabChargeSpeed = 4;
        [SerializeField] private float stabSpeed = 4;
        [Tooltip("-1 to 0 is the charge up animtion, 0 to 1 is the attack")]
        [SerializeField] private AnimationCurve stabPattern;


        [Header("Swing Attack")]
        [SerializeField] private float swingDamage = 4;
        [SerializeField] private float swingKnockBack = 8;
        [SerializeField] private float swingAngle = 222;
        [SerializeField] private float swingChargeSpeed = 1.5f;
        [SerializeField] private float swingSpeed = 6;
        [SerializeField] private AnimationCurve swingPattern;
        private float savedAngle;

        public override void UpdateWeapon()
        {
            if (attackState == 0)
            {
                savedAngle = controller.UpdateAiming();
                PlaceWeapon(savedAngle, baseDistance, drag);
            }
            else if (attackState == 1)
            {
                savedAngle = controller.UpdateAiming();
                PlaceWeapon(savedAngle, baseDistance, drag * .65f);
            }
            WeaponInputs();
        }

        #region AttackPatterns
        private float attackTimer;
        #region StabAttack
        public override void AttackOneHold()
        {
            chargeUpOne += Time.deltaTime * stabChargeSpeed;

            float disMult = 1 + stabPattern.Evaluate(-chargeUpOne) * stabDis;

            chargeUpOne = Mathf.Clamp01(chargeUpOne);


            savedAngle = controller.UpdateAiming();
            float dir = savedAngle;
            //Add shake to show fully charged
            if (chargeUpOne > .9f)
            {
                dir += Random.RandomRange(-44, 44);
            }

            PlaceWeapon(dir, baseDistance * disMult, drag * .65f);
        }
        public override void AttackOneAct()
        {
            StartCoroutine(StabAttackAnim());
        }

        IEnumerator StabAttackAnim()
        {
            //Stops player making new attacks
            attackState = 3;
            attackTimer = 1 - chargeUpOne;

            activeDamage = stabDamage * Mathf.Clamp(chargeUpOne, 0.1f, 1);
            active = true;

            while (attackTimer < 1)
            {
                if (attackTimer < 0)
                {
                    //Leave charge quickly
                    attackTimer += Time.deltaTime * stabSpeed * 4;
                }
                else
                {
                    attackTimer += Time.deltaTime * stabSpeed;
                }

                float AnimDis = stabPattern.Evaluate(attackTimer) * stabDis * chargeUpOne;
                PlaceWeapon(savedAngle, AnimDis, drag * 3);
                yield return null;
            }
            ResetInputs();
        }
        #endregion StabAttack
        #region SwingAttack
        public override void AttackTwoHold()
        {
            chargeUpTwo += Time.deltaTime * swingChargeSpeed;
            chargeUpTwo = Mathf.Clamp01(chargeUpTwo);

            //Invert the hold direction and clamp it to limited range
            savedAngle = controller.UpdateAiming();
            float dir = -savedAngle;
            if (dir < 0)
            {
                // dir = Mathf.Clamp(dir, -100, -80);
                dir = Mathf.Clamp(dir, -80, -100);
            }
            else
            {
                // dir = Mathf.Clamp(dir, 80, 100);
                dir = Mathf.Clamp(dir, 100, 80);
            }
            //Add shake to show fully charged
            if (chargeUpTwo > .9f)
            {
                dir += Random.RandomRange(-12, 12);
            }

            PlaceWeapon(dir, baseDistance, drag * 5);
        }
        public override void AttackTwoAct()
        {
            StartCoroutine(SwingAttackAnim());
        }

        IEnumerator SwingAttackAnim()
        {
            //Stops player making new attacks
            attackState = 3;
            attackTimer = 0;

            activeDamage = swingDamage * Mathf.Clamp(chargeUpTwo, 0.1f, 1);
            active = true;

            while (attackTimer < 2)
            {
                if (attackTimer < 0)
                {
                    //Leave charge quickly
                    attackTimer += Time.deltaTime * swingSpeed;// * 4;
                }
                else
                {
                    attackTimer += Time.deltaTime * swingSpeed;
                }

                float AnimDis = -((swingAngle * chargeUpTwo) / 2)
                    + swingPattern.Evaluate(attackTimer) * swingAngle * chargeUpTwo;

                //Make sword allways swing up
                if (savedAngle > 0)
                {
                    AnimDis = -AnimDis;
                }

                PlaceWeapon(savedAngle + AnimDis, baseDistance, drag * 12);
                PlaceWeaponSpecific(savedAngle + AnimDis, baseDistance, drag * 6, savedAngle + AnimDis, drag * 32);
                yield return null;
            }
            ResetInputs();
        }
        #endregion SwingAttack

        #endregion AttackPatterns
        public override void ResetInputs()
        {
            base.ResetInputs();
            attackTimer = 0;
            activeDamage = 0;
            active = false;
        }

        public override void ApplyDamage(Core.Health Target)
        {
            Target.Damage((int)activeDamage);
            if (Target.GetComponent<Rigidbody2D>())
            {
                //Get veleoctiy and use that to apply force direction
                if (chargeUpOne != 0)
                {
                    Target.GetComponent<Rigidbody2D>().AddForceAtPosition(stabKnockBack * chargeUpOne * transform.up, transform.position, ForceMode2D.Impulse);

                }
                else
                {
                    Target.GetComponent<Rigidbody2D>().AddForceAtPosition(swingKnockBack * chargeUpTwo * -transform.right, transform.position, ForceMode2D.Impulse);

                }
            }
        }

        public override void ResetObject()
        {
            ResetInputs();
        }
    }
}