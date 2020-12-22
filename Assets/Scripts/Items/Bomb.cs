using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

namespace Assets.Scripts.Items
{
    public class Bomb : MonoBehaviour
    {
        public float explosionRadius;
        [SerializeField]
        public float damage;
        private Player player;
        private Zombie zombie;
        private Animator animator;

        public LayerMask damageLayer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            player = FindObjectOfType<Player>();
        }

        private void Explode()
        {  
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    player.LoseHp(damage);
                }
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    zombie = collider.GetComponent<Zombie>();
                    zombie.LoseHp(damage);
                }
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
