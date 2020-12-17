using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Zombie : MonoBehaviour
    {
        public float moveRadius = 10;
        public float attackRadius = 3;

        Player player;

        ZombieState activeState;

        Animator animator;
        ZombieMovement movement;

        enum ZombieState
        {
            STAND,
            MOVE,
            ATTACK
        }
        private void Awake()
        {
            animator = GetComponent<Animator>();
            movement = GetComponent<ZombieMovement>();
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
            activeState = ZombieState.STAND;
        }

        void Update()
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            switch (activeState)
            {
                case ZombieState.STAND:
                    if (distance < moveRadius)
                    {
                        activeState = ZombieState.MOVE;
                        return;
                    }
                    movement.enabled = false;
                    break;
                case ZombieState.MOVE:
                    if (distance < attackRadius)
                    {
                        activeState = ZombieState.ATTACK;
                        return;
                    }
                    animator.SetFloat("Speed", 1);
                    movement.enabled = true;
                    break;
                case ZombieState.ATTACK:
                    if (distance > attackRadius)
                    {
                        activeState = ZombieState.MOVE;
                        return;
                    }
                    animator.SetTrigger("Shoot");
                    movement.enabled = false;
                    break;
            }
        }

            private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, moveRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
        }
    }
