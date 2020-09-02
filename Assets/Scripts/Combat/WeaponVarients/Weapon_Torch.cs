using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.Combat
{
    public class Weapon_Torch : WeaponBase
    {
        [Header("Boom Attack")]
        [SerializeField] private float boomDamage = 3;
        [SerializeField] private float boomKnockBack = 7;
        [SerializeField] private float boomChargeSpeed = 1;
        [SerializeField] private float boomSpeed = 2;
        [Tooltip("-1 to 0 is the charge up animtion, 0 to 1 is the attack")]
        [SerializeField] private AnimationCurve boomAnimPattern;
        [SerializeField] private AnimationCurve boomRaduis;
        [SerializeField] private WeaponB_DeathBurst deathSphere;


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
                float endAngle = savedAngle;
                if (endAngle < 0)
                {
                    endAngle = Mathf.Clamp(endAngle, -45, 45);
                }
                else
                {
                    endAngle = Mathf.Clamp(endAngle, -45, 45);
                }

                PlaceWeaponSpecific(savedAngle, baseDistance, drag, endAngle, endPointDrag);
            }
            WeaponInputs();
        }

        #region AttackPatterns
        private float attackTimer;
        #region StabAttack
        public override void AttackOneHold()
        {
            chargeUpOne += Time.deltaTime * boomChargeSpeed;
            chargeUpOne = Mathf.Clamp01(chargeUpOne);

            float disMult = 1 + boomAnimPattern.Evaluate(-chargeUpOne) * .6f;

            //Invert the hold direction and clamp it to limited range
            savedAngle = controller.UpdateAiming();

            savedAngle = Mathf.Clamp(savedAngle, -12, 12);

            float endAngle = savedAngle;

            endAngle = Mathf.Clamp(endAngle, -12, 12);

            //Add shake to show fully charged
            if (chargeUpOne > .9f)
            {
                endAngle += Random.RandomRange(-55, 55);
            }

            PlaceWeaponSpecific(savedAngle, disMult, drag, endAngle, endPointDrag);
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

            //We're using another object to apply damage, in the future this should spawn a projectile instead
            activeDamage = boomDamage * Mathf.Clamp(chargeUpOne, 0.1f, 1);
            deathSphere.SetDamage(activeDamage);
            deathSphere.SetBaseForce(boomKnockBack);
            deathSphere.transform.parent.transform.localScale = Vector3.one * .1f;
            deathSphere.transform.parent.gameObject.SetActive(true);

            while (attackTimer < 1)
            {
                if (attackTimer < 0)
                {
                    //Leave charge quickly
                    attackTimer += Time.deltaTime * boomSpeed * 4;
                }
                else
                {
                    attackTimer += Time.deltaTime * boomSpeed;
                }

                //Reduce puch force over time
                deathSphere.forceMult = Mathf.Clamp01((1 - attackTimer)) * chargeUpOne;

                float AnimDis = boomAnimPattern.Evaluate(attackTimer) * .6f * chargeUpOne;
                PlaceWeapon(savedAngle, AnimDis, drag * 12);
                deathSphere.transform.parent.transform.localScale = Vector3.one * AnimDis * boomRaduis.Evaluate(chargeUpOne);

                yield return null;
            }
            deathSphere.transform.parent.gameObject.SetActive(false);
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

            savedAngle = controller.UpdateAiming();
            float endAngle = savedAngle;
            if (endAngle < 0)
            {
                endAngle = Mathf.Clamp(endAngle, 45, -45);
            }
            else
            {
                endAngle = Mathf.Clamp(endAngle, 45, -45);
            }
            //Add shake to show fully charged
            if (chargeUpTwo > .9f)
            {
                endAngle += Random.RandomRange(-65, 65);
            }
            PlaceWeaponSpecific(dir, baseDistance, drag, endAngle, endPointDrag);
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
                    Target.GetComponent<Rigidbody2D>().AddForceAtPosition(boomKnockBack * chargeUpOne * transform.up, transform.position, ForceMode2D.Impulse);

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