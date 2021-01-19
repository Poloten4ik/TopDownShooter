using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Lean.Pool;

namespace Assets.Scripts.Items
{
    public class Bomb : MonoBehaviour
    {
        public float explosionRadius;
        public float damage;
        public GameObject explosionPrefab;
        public LayerMask damageLayer;

        private void Explode()
        {
            LeanPool.Spawn(explosionPrefab, transform.position, Quaternion.identity);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);
            foreach (Collider2D collider in colliders)
            {
                collider.gameObject.SendMessage("LoseHp", damage);
            }

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
                LeanPool.Despawn(gameObject);
            }
        }

    }
}
