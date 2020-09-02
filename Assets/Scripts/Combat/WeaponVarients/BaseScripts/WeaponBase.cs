using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.Combat
{
    public class WeaponBase : WeaponObject
    {
        //WeaponAttacks&Inputs
        public override void UpdateWeapon()
        {
            base.UpdateWeapon();
            WeaponInputs();

        }

        #region AttackInputs
        protected int attackState; //0 is idle, 1 & 2 are attacks and 3 is disabled

        protected float chargeUpOne;
        protected float chargeUpTwo;
        public virtual void WeaponInputs()
        {
            if (attackState != 2 && attackState != 3)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ResetInputs();
                    attackState = 1;
                }
                else if (Input.GetMouseButton(0))
                {
                    AttackOneHold();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    AttackOneAct();
                }
            }
            if (attackState != 1 && attackState != 3)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    ResetInputs();
                    attackState = 2;
                }
                else if (Input.GetMouseButton(1))
                {
                    AttackTwoHold();
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    AttackTwoAct();
                }
            }
        }


        #region AttackPatternOneChargeUp
        public virtual void AttackOneHold()
        {
            chargeUpOne += Time.deltaTime;
        }
        public virtual void AttackOneAct()
        {
            attackState = 3;
            ResetInputs();
        }
        #endregion AttackPatternOneChargeUp
        #region AttackPatternTwoChargeUp
        public virtual void AttackTwoHold()
        {
            chargeUpTwo += Time.deltaTime;
        }
        public virtual void AttackTwoAct()
        {
            attackState = 3;
            ResetInputs();
        }
        #endregion AttackPatternTwoChargeUp

        #endregion AttackInputs

        public virtual void ResetInputs()
        {
            attackState = 0;
            chargeUpOne = 0;
            chargeUpTwo = 0;
        }
    }
}