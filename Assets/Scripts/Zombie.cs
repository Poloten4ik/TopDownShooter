using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

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
        private AIPath aiPath;
        AIDestinationSetter aIDestinationSetter;
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
            aiPath = GetComponent<AIPath>();
            rb = GetComponent<Rigidbody2D>();
            autoDestroyer = GetComponent<AutoDestroyer>();
            aIDestinationSetter = GetComponent<AIDestinationSetter>();

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
                    aiPath.enabled = false;
                    break;

                case ZombieState.RETURN:
                   
                    aiPath.enabled = true;
                    break;

                case ZombieState.MOVE_TO_PLAYER:
                    aIDestinationSetter.target = player.transform;
                    aiPath.enabled = true;
                    break;

                case ZombieState.ATTACK:
                    aiPath.enabled = false;
                    break;

                case ZombieState.DEATH:
                    animator.SetTrigger("Death");
                    aiPath.enabled = false;
                    break;
            }
            activeState = newState;
        }

        private void DoStand()
        {
            CheckMoveToPlayer();
        }

        private void DoMove()
        {
            if (distanceToPlayer < attackRadius)
            {
                ChangeState(ZombieState.ATTACK);
                animator.SetFloat("Speed", 0);
                return;
            }

            if(distanceToPlayer > followRadius) 
            {
                ChangeState(ZombieState.RETURN);
                animator.SetFloat("Speed", 0);
                return;
                  
            }
            animator.SetFloat("Speed", 1);
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

        private void DoAttack()
        {
            if (distanceToPlayer > attackRadius)
            {
                ChangeState(ZombieState.MOVE_TO_PLAYER);
                print("Attack");
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

            Gizmos.color = Color.magenta;
            Vector3 lookDirecttion = -transform.up;
           
            Vector3 leftViewVector = Quaternion.AngleAxis(viewAngle / 2 , Vector3.forward) * lookDirecttion;
            Vector3 rightViewVector = Quaternion.AngleAxis(-viewAngle / 2, Vector3.forward) * lookDirecttion;


            Gizmos.DrawRay(transform.position, leftViewVector * followRadius);
            Gizmos.DrawRay(transform.position, rightViewVector * followRadius);

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
