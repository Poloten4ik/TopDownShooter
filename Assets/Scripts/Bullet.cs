using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Items;
using Lean.Pool;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 20f;
        public float damage;

        private Rigidbody2D rb;
        private Player player;
        private Enemy enemy;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            rb.velocity = -transform.up * speed;
        }

        private void OnBecameInvisible()
        {
            LeanPool.Despawn(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            LeanPool.Despawn(gameObject);  
        }
    }

    // TODO понять как работает 
    public static class Extensions
    {
        public static bool CompareTag(this Collision2D obj, string tag)
        {
            return obj.gameObject.CompareTag(tag);
        }
    }
}
