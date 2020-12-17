using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ZombieMovement : MonoBehaviour
    {
        public int speed;

        Rigidbody2D rb;
        Animator animator;
        Player player;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
        }

        void Update()
        {
            

            Move();
            Rotate();
        }

        private void Move()
        {
            Vector3 zombiePosition = transform.position;
            Vector3 playerPosition = player.transform.position;

            Vector3 direction = playerPosition - zombiePosition;

            if (player.isPlayerAlive)
            {
                
                if (direction.magnitude > 1)
                {
                    direction = direction.normalized;
                }

                animator.SetFloat("Speed", direction.magnitude);

                rb.velocity = direction.normalized * speed;
            }

            else
            {
                rb.velocity = Vector2.zero;
            }

        }

        private void Rotate()
        {
            Vector3 zombiePosition = transform.position;
            Vector3 playerPosition = player.transform.position;

            Vector3 direction = playerPosition - zombiePosition;

            if (player.isPlayerAlive)
            {
                direction.z = 0;
                transform.up = -direction;
            }
        }

        private void OnDisable()
        {
            rb.velocity = Vector2.zero;
        }
    }
}

