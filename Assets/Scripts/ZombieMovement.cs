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

        public Vector3 zombiePosition;
        public Vector3 targetPosition;

        private ZombieSpawner zombieSpawner;
        private Player player;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            zombie = GetComponent<Zombie>();
     
        }

        private void Start()
        {
            //zombieSpawner = FindObjectOfType<ZombieSpawner>();
            player = FindObjectOfType<Player>();
           
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

            //if (Vector2.Distance(transform.position, zombieSpawner.wayPoints[zombieSpawner.wayPointIndex].position) < 3f)
            //{
            //    if (zombieSpawner.wayPointIndex < zombieSpawner.wayPoints.Length - 1)
            //    {
            //        zombieSpawner.wayPointIndex += 1;
            //        targetPosition = zombieSpawner.wayPoints[zombieSpawner.wayPointIndex].position;
            //    }
            //    else if (zombieSpawner.wayPointIndex >= zombieSpawner.wayPoints.Length)
            //    {
            //        zombie.ChangeState(Zombie.ZombieState.MOVE_TO_PLAYER);
            //    }

            //}
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

