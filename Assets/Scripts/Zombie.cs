using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Zombie : MonoBehaviour
    {
        [SerializeField]
        private float moveRadius = 10;
        [SerializeField]
        private float attackRadius = 3;
        [SerializeField]
        private float followRadius = 8;
        [SerializeField]
        private float damage = 25;
        [SerializeField]
        private float health = 100;
        [HideInInspector]
        public bool isAlive = true;
        [SerializeField]
        private float attackRate = 1f;
        private float nextAttack;

        private float distanceToPlayer;

        private Player player;
        private Bullet bullet;
        private Animator animator;

        public ZombieState activeState;
        private ZombieMovement movement;
        private Rigidbody2D rb;

        public Vector3 startPosition;

        public enum ZombieState
        {
            STAND,
            RETURN,
            MOVE_TO_PLAYER,
            ATTACK,
            DEATH
        }
        private void Awake()
        {
            animator = GetComponent<Animator>();
            movement = GetComponent<ZombieMovement>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
            activeState = ZombieState.STAND;
            startPosition = transform.position;
        }

        private void Update()
        {
            if (!isAlive)
            {
                activeState = ZombieState.DEATH;
            }

            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            switch (activeState)
            {
                case ZombieState.STAND:
                    DoStand();
                    break;

                case ZombieState.RETURN:
                    DoReturn();
                    break;

                case ZombieState.MOVE_TO_PLAYER:
                    DoMove();
                    break;

                case ZombieState.ATTACK:
                    DoAttack();
                    break;

                case ZombieState.DEATH:
                    animator.SetTrigger("Death");
                    movement.enabled = false;
                    return;
            }
        }

        private void DoStand()
        {
            if (distanceToPlayer < moveRadius)
            {
                activeState = ZombieState.MOVE_TO_PLAYER;
                return;
            }
            animator.SetTrigger("Idle");
            movement.enabled = false;
        }

        private void DoReturn()
        {
            if (distanceToPlayer < moveRadius)
            {
                activeState = ZombieState.MOVE_TO_PLAYER;
                return;
            }

            float distanceToStart = Vector3.Distance(transform.position, startPosition);
            if (distanceToStart <= 0.05f)
            {
                activeState = ZombieState.STAND;
                return;
            }

            movement.targetPosition = startPosition;
            movement.enabled = true;

        }
        private void DoMove()
        {
            if (distanceToPlayer < attackRadius)
            {
                activeState = ZombieState.ATTACK;
                return;
            }
            if (distanceToPlayer > followRadius)
            {
                activeState = ZombieState.RETURN;
                return;
            }
            movement.targetPosition = player.transform.position;
            movement.enabled = true;
        }

        private void DoAttack()
        {
            if (distanceToPlayer > attackRadius)
            {
                activeState = ZombieState.MOVE_TO_PLAYER;
                return;
            }
            movement.enabled = false;

            nextAttack -= Time.deltaTime;
            if (nextAttack <= 0)
            {
                animator.SetTrigger("Shoot");
                nextAttack = attackRate;
            }
        }

        private void DamageToPlayer()
        {
            player.LoseHp(damage);
        }

        public void LoseHp(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                isAlive = false;
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, moveRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, followRadius);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                bullet = collision.gameObject.GetComponent<Bullet>();
                LoseHp(bullet.damage);
            }

        }
    }
}
