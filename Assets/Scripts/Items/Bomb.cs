using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Scripts.Items
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField]
        private float explosionRadius;
        [SerializeField]
        private float damage;
        private Player player;
        private Zombie zombie;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        void Start()
        {
        }

        void Update()
        {

        }

        private void Explode()
        {
            player = FindObjectOfType<Player>();

            int layerMask = LayerMask.GetMask("Enemy");
            

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerMask);
            foreach (Collider2D collider in colliders)
            {
                zombie = collider.GetComponent<Zombie>();
                zombie.LoseHp(damage);
            }
            animator.SetTrigger("Explosion");
           
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                Explode();
            }
        }
        public void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
