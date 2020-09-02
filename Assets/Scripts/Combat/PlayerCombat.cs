using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.Combat
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField]
        private HeldObject currentWeapon;

        private void Start()
        {
            if (currentWeapon != null)
            {
                currentWeapon.AttachWeapon(this);
            }
        }
        void Update()
        {
            if (currentWeapon == null)
            {
                return;
            }
            currentWeapon.UpdateWeapon();
        }

        #region WeaponSwapping
        public void SwapWeapon(HeldObject NewTarget)
        {
            if (currentWeapon != null)
            {
                //Hide current weapon (Once we have a inventory system this should use that)
                currentWeapon.gameObject.SetActive(false);
                //This can be used to reset any weapon stats aka make sure weapon isn't mid attack
                currentWeapon.HideObject();
                currentWeapon = null;
            }
            if (NewTarget != null)
            {
                currentWeapon = NewTarget;
                currentWeapon.AttachWeapon(this);
                currentWeapon.ResetObject();
                currentWeapon.gameObject.SetActive(true);
            }
        }

        #endregion WeaponSwapping

        //Weapons use these to work out their position and rotations
        #region AimingFunctions
        public float UpdateAiming()
        {
            Vector3 mouseDir = GetMousePosistion() - transform.position;
            //Get the target direction as a angle so we can apply it via rotation
            float mouseAngle = Vector3.Angle(Vector3.up, mouseDir);
            if (mouseDir.x > 0)
            {
                mouseAngle = -mouseAngle;
            }
            return mouseAngle;
        }
        Vector3 GetMousePosistion()
        {
            Plane plane = new Plane(Vector3.forward, transform.position);

            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 MousePos = transform.position;
            if (plane.Raycast(ray, out distance))
            {
                MousePos = ray.GetPoint(distance);
            }
            return MousePos;
        }
        #endregion AimingFunctions
    }
}