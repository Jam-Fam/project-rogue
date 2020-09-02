using ProjectRogue.Movement;
using UnityEngine;

namespace ProjectRogue.PlayerInput
{
    public class TempInput : MonoBehaviour
    {
        CharacterController2D charControl;

        public float inputThreshold;
        public float crouchThreshold = -0.5f;
        float inputX;
        bool jumped;
        bool isCrouching;

        // Start is called before the first frame update
        void Start()
        {
            charControl = GetComponent<CharacterController2D>();
        }

        // Update is called once per frame
        void Update()
        {
            inputX = Mathf.Abs(Input.GetAxisRaw("Horizontal")) >= inputThreshold ? Mathf.Sign(Input.GetAxisRaw("Horizontal")) : 0;
            // Set the value directly to 1 or -1 as long as the input passes the joystick press threshold.

            if (Input.GetButtonDown("Jump"))
                jumped = true;

            if (Input.GetButtonUp("Jump"))
                charControl.jumpButtonReleased = true;

            isCrouching = Input.GetAxisRaw("Vertical") < crouchThreshold;
        }

        private void FixedUpdate()
        {
            charControl.Move(inputX, isCrouching, jumped);
            jumped = false;
        }
    }
}