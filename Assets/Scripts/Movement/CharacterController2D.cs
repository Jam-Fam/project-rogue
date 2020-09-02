using UnityEngine;
using UnityEngine.Events;

namespace ProjectRogue.Movement
{
    public class CharacterController2D : MonoBehaviour
    {

        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothingGround = .05f;  // Ground smoothing
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothingAir = .05f;  // Air Smoothing
        [SerializeField] private float movementSpeed;
        [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
        [SerializeField] private Collider2D m_UpperCollider;                        // A collider that will be disabled when crouching
        [SerializeField] private Collider2D m_LowerCollider;                    // My added collider.


        [SerializeField] private float groundedRaycastDistance;
        public bool m_Grounded;            // Whether or not the player is grounded. >>>> I made public when it was private <<<<<<
        [HideInInspector] public Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private Vector3 m_Velocity = Vector3.zero;
        private bool hasPositiveJumpVelocity;
        [SerializeField] private int airJumps;
        private int jumpsRemaining;
        private float smoothingAppliedToMovement;
        private float verticalVelocity;
        [SerializeField] private float groundedVerticalVelocity;
        private bool fallingAfterJump;
        private bool currentlyJumping;
        [SerializeField] private float jumpForce = 400f;
        [SerializeField] private float gravity;
        [SerializeField] private float gravityLerpRate;
        [SerializeField] private AnimationCurve JumpVelocityCurve;
        [SerializeField] private float earlyJumpThreshold;
        private float jumpCurveEvaluation;
        public bool jumpButtonReleased;
        [SerializeField] private float jumpReleaseCutoffThreshold;
        [SerializeField] private float jumpReleaseLerpRate;

        public Animator Animator;

        private enum JumpState
        {
            defaultJump,
            earlyButtonRelease,
            normalButtonRelease
        }

        JumpState jumpState;

        [Header("Events")]
        [Space]

        public UnityEvent OnLandEvent;

        [System.Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        public BoolEvent OnCrouchEvent;
        private bool m_wasCrouching = false;

        [Header("Debug")]
        [SerializeField] private bool enableDebugGizmos = true;


        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();

            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();

            if (OnCrouchEvent == null)
                OnCrouchEvent = new BoolEvent();

            jumpsRemaining = airJumps;
        }

        private void Update()
        {
            if (currentlyJumping)
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            PlayerGroundedCheck();

            if (!m_Grounded && !currentlyJumping)
            {
                // Add gravity effect to velocity here.
            }

            if (hasPositiveJumpVelocity && m_Rigidbody2D.velocity.y <= 0f)
                hasPositiveJumpVelocity = false;
        }


        public void Move(float move, bool crouch, bool jump)
        {
            Animator.SetBool("isRunning", move == 0f ? false : true);

            #region HorizontalMovement
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {

                // Move the character by finding the target velocity
                Vector3 targetVelocity = new Vector2(move * movementSpeed, verticalVelocity);

                m_Rigidbody2D.velocity = new Vector2(Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, smoothingAppliedToMovement).x, targetVelocity.y);
                // SmoothDamp from Brackeys script added for the X value and a direct valocity input for the Y value.

                if (move > 0 && !m_FacingRight)
                {
                    Flip();
                }
                else if (move < 0 && m_FacingRight)
                {
                    Flip();
                }
            }
            #endregion

            #region Jumping
            if ((m_Grounded || jumpsRemaining > 0) && jump)
            {
                if (!m_Grounded)
                {
                    jumpsRemaining--;
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
                    fallingAfterJump = false;
                    //Air jump code resets vertical velocity before adding another force so you don't skyrocket due to the first jump's force.
                }
                m_Grounded = false;
                currentlyJumping = true;
                hasPositiveJumpVelocity = true;
                jumpCurveEvaluation = 0f;
                jumpButtonReleased = false;
                jumpState = JumpState.defaultJump;
            }
            #endregion
        }


        private void Flip()
        {
            m_FacingRight = !m_FacingRight;
            transform.Rotate(0f, 180, 0f);
            // Rotate the player 180 degrees on the Y axis when the player switches directions.
        }

        private void PlayerGroundedCheck()
        {
            bool wasGrounded = m_Grounded;
            m_Grounded = false;
            smoothingAppliedToMovement = m_MovementSmoothingAir;

            if (!currentlyJumping)
            {
                verticalVelocity = Mathf.Lerp(verticalVelocity, gravity, gravityLerpRate);
            }

            // The raycast position vectors can be set once in Awake/Start if we decide colliders will never change size during runtime.
            Vector2 raycastCornerRight = new Vector2(m_LowerCollider.bounds.max.x, m_LowerCollider.bounds.min.y);
            Vector2 raycastCornerLeft = m_LowerCollider.bounds.min;
            Vector2 raycastCenter = new Vector2(m_LowerCollider.bounds.center.x, m_LowerCollider.bounds.min.y);
            if
                ((
                Physics2D.Raycast(raycastCornerLeft, Vector2.down, groundedRaycastDistance, m_WhatIsGround)
                ||
                Physics2D.Raycast(raycastCornerRight, Vector2.down, groundedRaycastDistance, m_WhatIsGround)
                ||
                Physics2D.Raycast(raycastCenter, Vector2.down, groundedRaycastDistance, m_WhatIsGround)
                )
                &&
                !hasPositiveJumpVelocity
                )
            {
                m_Grounded = true;
                smoothingAppliedToMovement = m_MovementSmoothingGround;

                if (!currentlyJumping)
                    verticalVelocity = groundedVerticalVelocity;

                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
            // Grounds player and invokes OnLandEvent if the raycasts detect ground and the player is not currently jumping upwards.
        }

        void Jump()
        {
            switch (jumpState)
            {
                case JumpState.normalButtonRelease:
                    verticalVelocity = Mathf.Lerp(verticalVelocity, 0f, jumpReleaseLerpRate * Time.deltaTime);
                    if (verticalVelocity <= jumpReleaseCutoffThreshold)
                        currentlyJumping = false;
                    break;

                case JumpState.earlyButtonRelease:
                    // Stopped near beginning.
                    // Let velocity follow new curve using current jumpCurveEvaluation.
                    break;

                case JumpState.defaultJump:
                    verticalVelocity = Mathf.Lerp(0f, jumpForce, JumpVelocityCurve.Evaluate(jumpCurveEvaluation));
                    break;

                default:
                    print("Default jumpState switch value. jumpState = " + jumpState);
                    break;
            }

            if (verticalVelocity <= 0f)
                currentlyJumping = false;

            if (jumpButtonReleased)
            {
                jumpState = jumpCurveEvaluation > earlyJumpThreshold ? JumpState.normalButtonRelease : JumpState.earlyButtonRelease;
                jumpButtonReleased = false;
            }
            jumpCurveEvaluation += Time.deltaTime;

        }

        public void ResetAirJumps() // Called from OnLand event.
        {
            jumpsRemaining = airJumps;
            // Probably add this to some bigger and more whole landing sequence later.
        }



        // DEBUG RELATED METHODS BELOW THIS POINT V V V V V

        private void OnDrawGizmosSelected()
        {
            if (enableDebugGizmos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(m_LowerCollider.bounds.min, .01f);

                Gizmos.DrawWireSphere(new Vector2(m_LowerCollider.bounds.center.x, m_LowerCollider.bounds.min.y), .01f); //New
                Debug.DrawRay(new Vector2(m_LowerCollider.bounds.center.x, m_LowerCollider.bounds.min.y), Vector2.down * groundedRaycastDistance, Color.red);

                Gizmos.DrawWireSphere(new Vector2(m_LowerCollider.bounds.max.x, m_LowerCollider.bounds.min.y), .01f);
                Debug.DrawRay(m_LowerCollider.bounds.min, Vector2.down * groundedRaycastDistance, Color.red);
                Debug.DrawRay(new Vector2(m_LowerCollider.bounds.max.x, m_LowerCollider.bounds.min.y), Vector2.down * groundedRaycastDistance, Color.red);
                // Ground check raycasts from the lower corners of the bottom collider. WireSphere rendered at origins.
            }
        }
    }
}