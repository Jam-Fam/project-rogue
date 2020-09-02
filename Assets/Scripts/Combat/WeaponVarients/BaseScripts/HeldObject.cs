using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRogue.Combat
{
    public class HeldObject : MonoBehaviour
    {
        [Header("Holding Settings")]
        [SerializeField] protected AnimationCurve holdCircle;
        [SerializeField] protected float baseDistance = .8f;
        [SerializeField] protected float drag = 8;
        [Range(.1f, 5)]
        [SerializeField] protected float endPointDrag = .9f;

        protected PlayerCombat controller;

        public void AttachWeapon(PlayerCombat NewController)
        {
            controller = NewController;
            //Attach
            transform.parent = NewController.transform;
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;

            baseTarget = transform.rotation;
            endTarget = transform.rotation;
        }
        public virtual void UpdateWeapon()
        {
            PlaceWeapon(controller.UpdateAiming(), baseDistance, drag);

        }

        Quaternion baseTarget;
        Quaternion endTarget;
        public virtual void PlaceWeapon(float TargetAngle, float TargetDistance, float Speed)
        {
            //Lerp with drag old rotations to new aim rotations
            Quaternion targetRotation = Quaternion.AngleAxis(TargetAngle, Vector3.forward);
            baseTarget = Quaternion.Lerp(baseTarget, targetRotation, Time.deltaTime * Speed);
            endTarget = Quaternion.Lerp(endTarget, targetRotation, Time.deltaTime * Speed * endPointDrag);

            //Convert Rotations to direction and place weapon base a set distance from body and it's rotation
            transform.position = controller.transform.position + (baseTarget * Vector3.up)
                * (holdCircle.Evaluate(Mathf.Abs(TargetAngle / 180)) * (TargetDistance));
            transform.rotation = endTarget;
        }
        public virtual void PlaceWeaponSpecific(float TargetAngle, float TargetDistance, float Speed, float EndAngle, float EndDragMult)
        {
            //Lerp with drag old rotations to new aim rotations 
            baseTarget = Quaternion.Lerp(baseTarget, Quaternion.AngleAxis(TargetAngle, Vector3.forward)
                , Time.deltaTime * Speed);
            endTarget = Quaternion.Lerp(endTarget, Quaternion.AngleAxis(EndAngle, Vector3.forward)
                , Time.deltaTime * Speed * EndDragMult);

            //Convert Rotations to direction and place weapon base a set distance from body and it's rotation
            transform.position = controller.transform.position + (baseTarget * Vector3.up)
                * (holdCircle.Evaluate(Mathf.Abs(TargetAngle / 180)) * (TargetDistance));
            transform.rotation = endTarget;
        }

        public virtual void HideObject()
        {

        }
        public virtual void ResetObject()
        {

        }

    }
}