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
        private Zombie zombie;

        [HideInInspector]
        public bool isFollow = false;

        [HideInInspector]
        public bool isPositionStart = true;

        public Vector3 startPosition;
        public Vector3 zombiePosition;
        public Vector3 targetPosition;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            startPosition = transform.position;
            zombie = GetComponent<Zombie>();
        }

        private void Start()
        {

        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            Vector3 zombiePosition = transform.position;
            Vector3 direction = targetPosition - zombiePosition;

            if (direction.magnitude > 1)
            {
                direction = direction.normalized;
            }

            animator.SetFloat("Speed", direction.magnitude);

            rb.velocity = direction * speed;
        }

        private void Rotate()
        {

            Vector3 zombiePosition = transform.position;

            Vector3 direction = targetPosition - zombiePosition;

            direction.z = 0;
            transform.up = -direction;

        }

        private void OnDisable()
        {
            rb.velocity = Vector2.zero;
        }
    }
}

