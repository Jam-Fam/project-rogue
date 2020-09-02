using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.Combat
{
    public class WeaponSwapper : MonoBehaviour
    {
        [SerializeField]
        private PlayerCombat combatController;
        [SerializeField]
        private HeldObject[] weapons;
        private int currentSlot;
        private void Start()
        {
            combatController.SwapWeapon(weapons[currentSlot]);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                NextSlot();
            }
        }

        void NextSlot()
        {
            currentSlot += 1;
            if (currentSlot >= weapons.Length)
            {
                currentSlot = 0;
            }
            combatController.SwapWeapon(weapons[currentSlot]);
        }
    }
}