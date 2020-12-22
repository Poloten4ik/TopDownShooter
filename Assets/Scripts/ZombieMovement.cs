using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ZombieMovement : MonoBehaviour
    {
        public int speed;

        private Rigidbody2D rb;
        private Animator animator;
        private Player player;

        [HideInInspector]
        public bool isAlive = true;

        [HideInInspector]
        public bool isFollow = false;

        [HideInInspector]
        public bool isPositionStart = true;

        public Vector3 startPosition;
        public Vector3 zombiePosition;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            startPosition = transform.position;
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
            StartPosition();
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            Vector3 zombiePosition = transform.position;
            Vector3 playerPosition = player.transform.position;
            Vector3 direction = playerPosition - zombiePosition;
            if (isAlive)
            {
                if (player.isPlayerAlive && isFollow)
                {

                    if (direction.magnitude > 1)
                    {
                        direction = direction.normalized;
                    }

                    animator.SetFloat("Speed", direction.magnitude);
                    rb.velocity = direction.normalized * speed;
                }

                else if (!isFollow)
                {
                    Vector3 direction2 = zombiePosition - startPosition;

                    animator.SetFloat("Speed", direction2.magnitude);
                    rb.velocity = -direction2.normalized * speed;
                }
            }
        }

        private void Rotate()
        {
            if (isAlive)
            {
                Vector3 zombiePosition = transform.position;
                Vector3 playerPosition = player.transform.position;

                Vector3 direction = playerPosition - zombiePosition;

                if (player.isPlayerAlive && isFollow)
                {
                    direction.z = 0;
                    transform.up = -direction;
                }
                else if (!isFollow)
                {
                    Vector3 direction2 = zombiePosition - startPosition;
                    transform.up = direction2;
                    direction2.z = 0;
                }
            }
        }

        public void StartPosition()
        {
            Vector3 zombiePosition = transform.position;

            if (Vector3.Distance(zombiePosition, startPosition) < 0.2f)
            {
              //TODO
            }
        }

        private void OnDisable()
        {
            rb.velocity = Vector2.zero;
        }
    }
}

