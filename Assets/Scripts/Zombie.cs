using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    public class Zombie : MonoBehaviour
    {
        public Action HealthChanged = delegate { };

        [SerializeField]
        private float moveRadius = 10;
        [SerializeField]
        private float attackRadius = 3;
        [SerializeField]
        private float followRadius = 8;
        [SerializeField]
        private float damage = 25;
        public float health = 100;
        [HideInInspector]
        public bool isAlive = true;
        [SerializeField]
        private float attackRate = 1f;
        private float nextAttack;

        public int viewAngle = 90;


        private float distanceToPlayer;

        private Player player;
        private Bullet bullet;
        private Animator animator;
        private AutoDestroyer autoDestroyer;

        public ZombieState activeState;
        private ZombieMovement movement;
        private Rigidbody2D rb;

        private ZombieSpawner zombieSpawner;

        public Vector3 startPosition;

        public GameObject[] pickUps;

        [Range (0, 100)]
        public float pickUpChange;

        public enum ZombieState
        {
            STAND,
            MOVE_TO_PLAYER,
            RETURN,
            ATTACK,
            DEATH
        }
        private void Awake()
        {
            animator = GetComponent<Animator>();
            movement = GetComponent<ZombieMovement>();
            rb = GetComponent<Rigidbody2D>();
            autoDestroyer = GetComponent<AutoDestroyer>();

        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
            ChangeState(ZombieState.STAND);
            startPosition = transform.position;
        }

        private void Update()
        {
            if (!isAlive)
            {
                DoDeath();
            }

            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            switch (activeState)
            {
                case ZombieState.STAND:
                    DoStand();
                    break;

                case ZombieState.MOVE_TO_PLAYER:
                    DoMoveToPlayer();
                    break;

                case ZombieState.RETURN:
                    DoReturn();
                    break;

                case ZombieState.ATTACK:
                    DoAttack();
                    break;

                case ZombieState.DEATH:
                    DoDeath();
                    break;
            }
        }

        public void ChangeState(ZombieState newState)
        {
            switch (newState)

            {
                case ZombieState.STAND:
                    animator.SetTrigger("Idle");
                    movement.enabled = false;
                    break;

                case ZombieState.RETURN:
                    movement.targetPosition = startPosition;
                    movement.enabled = true;
                    break;

                case ZombieState.MOVE_TO_PLAYER:
                    movement.enabled = true;
                    break;

                case ZombieState.ATTACK:
                    movement.enabled = false;
                    break;

                case ZombieState.DEATH:
                    animator.SetTrigger("Death");
                    movement.enabled = false;
                    break;
            }
            activeState = newState;
        }

        private void DoStand()
        {
            CheckMoveToPlayer();
        }

        private bool CheckMoveToPlayer()
        {
            //проверям радиус
            if (distanceToPlayer > moveRadius)
            {
                return false;
            }

            //проверям препятствия
            Vector3 directionToPlayer = player.transform.position - transform.position;
            Debug.DrawRay(transform.position, directionToPlayer, Color.red);

            float angle = Vector3.Angle(-transform.up, directionToPlayer);
            if (angle > viewAngle/2)
            {
                return false;
            }

            LayerMask layerMask = LayerMask.GetMask("Obstacles");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, layerMask);
            if (hit.collider != null)
            {
                //есть коллайдер
                return false;

            }

            //бежать за игроком
            ChangeState(ZombieState.MOVE_TO_PLAYER);
            return true;

        }

            private void DoMoveToPlayer()
        {
            if (distanceToPlayer < attackRadius)
            {
                ChangeState(ZombieState.ATTACK);
                return;
            }
            if (distanceToPlayer > followRadius)
            {
                ChangeState(ZombieState.RETURN);
                return;
            }
            movement.targetPosition = player.transform.position;
        }

        private void DoAttack()
        {
            if (distanceToPlayer > attackRadius)
            {
                ChangeState(ZombieState.MOVE_TO_PLAYER);
                return;
            }
          
            nextAttack -= Time.deltaTime;
            if (nextAttack <= 0)
            {
                animator.SetTrigger("Shoot");
                nextAttack = attackRate;
            }
        }

        private void DoDeath()
        {
            ChangeState(ZombieState.DEATH);
            autoDestroyer.enabled = true;
        }

        //private void PickUpChange()
        //{
        //    float pickUpChangeRandom = UnityEngine.Random.Range(0, 100);
        //    if (pickUpChangeRandom < pickUpChange)
        //    {
        //        int r = UnityEngine.Random.Range(0, pickUps.Length);
        //        Instantiate(pickUps[r], transform.position, Quaternion.identity);
        //    }
        //}

        private void DoReturn()
        {
            if (CheckMoveToPlayer())
            {
                return;
            }

            float distanceToStart = Vector3.Distance(transform.position, startPosition);
            if (distanceToStart <= 0.05f)
            {
                ChangeState(ZombieState.STAND);
                return;
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
                //PickUpChange();
                isAlive = false;
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
            HealthChanged();
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
