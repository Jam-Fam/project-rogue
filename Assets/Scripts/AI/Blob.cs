
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ProjectRogue.Movement;

namespace ProjectRogue.AI
{
    public class Blob : MonoBehaviour
    {
        // Dependencies 
        private Transform player;

        // Components
        private CharacterController2D character;

        // State
        private bool jumped = false;
        private float moveDirection;

        void Awake()
        {
            character = GetComponent<CharacterController2D>();
            player = GameObject.FindWithTag("Player").transform;
        }


        void Update()
        {
            if (character.m_Grounded)
            {
                jumped = true;
                moveDirection = 0;

            }
            else
            {
                Vector2 directionToPlayer;
                moveDirection = player.position.x > transform.position.x ? 1 : -1;
            }
        }

        private void FixedUpdate()
        {
            character.Move(moveDirection, false, jumped);
            jumped = false;
        }
    }
}
