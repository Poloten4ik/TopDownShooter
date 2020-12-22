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
        [SerializeField]
        private float attackRate = 1f;
        private float nextAttack;

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
            MOVE,
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
            float distance = Vector3.Distance(transform.position, player.transform.position);

            switch (activeState)
            {
                case ZombieState.STAND:
                    if (distance < moveRadius)
                    {
                        activeState = ZombieState.MOVE;
                        movement.isFollow = true;
                        return;
                    }
                    else if (health <= 0)
                    {
                        activeState = ZombieState.DEATH;
                        return;
                    }
                    animator.SetTrigger("Idle");
                    movement.enabled = false;
                    break;

                case ZombieState.MOVE:
                    if (distance < attackRadius)
                    {
                        activeState = ZombieState.ATTACK;
                        return;
                    }
                    else if (distance > followRadius)
                    {
                        activeState = ZombieState.MOVE;
                        movement.isFollow = false;
                        return;
                    }
                    else if (!movement.isFollow)
                    {
                        activeState = ZombieState.STAND;
                    }
                    else if (health <= 0)
                    {
                        activeState = ZombieState.DEATH;
                        return;
                    }
                    movement.enabled = true;
                    break;

                case ZombieState.ATTACK:
                    if (distance > attackRadius)
                    {
                        activeState = ZombieState.MOVE;
                        return;
                    }
                    else if (nextAttack <= 0)
                    {
                        Attack();
                        return;
                    }
                    else if (health <= 0)
                    {
                        activeState = ZombieState.DEATH;
                        return;
                    }
                    animator.SetTrigger("Shoot");
                    movement.enabled = false;
                    break;

                case ZombieState.DEATH:
                    animator.SetTrigger("Death");
                    movement.enabled = false;
                    return;
            }


            if (nextAttack > 0)
            {
                nextAttack -= Time.deltaTime;
            }

        }

        public void Attack()
        {
            if (movement.isAlive)
            {
                player.LoseHp(damage);
                nextAttack = attackRate;
            }
        }



        public void LoseHp(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                movement.isAlive = false;
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
