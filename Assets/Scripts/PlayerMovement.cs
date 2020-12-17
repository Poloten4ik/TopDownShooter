 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public int speed;

        Rigidbody2D rb;
        Animator animator;
        Player player;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            player = FindObjectOfType<Player>();
        }

        void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            if (player.isPlayerAlive)
            {
                float inputX = Input.GetAxis("Horizontal");
                float inputY = Input.GetAxis("Vertical");

                Vector2 direction = new Vector2(inputX, inputY);

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
            if (player.isPlayerAlive)
            {
                Vector3 playerPosition = transform.position;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector3 direction = mousePosition - playerPosition;
                direction.z = 0;
                transform.up = -direction;
            }
        }
    }
}
