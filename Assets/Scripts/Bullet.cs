using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 20f;
        public float damage;

        Rigidbody2D rb;
        Player player;
        Enemy enemy;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb.velocity = -transform.up * speed;
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                player = collision.gameObject.GetComponent<Player>();
                player.LoseHp(damage);
                Destroy(gameObject);
            }

            if (collision.gameObject.CompareTag("Enemy"))
            {
                enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.LoseHp(damage);
                Destroy(gameObject);
            }
        }
    }

    public static class Extensions
    {
        public static bool CompareTag(this Collision2D obj, string tag)
        {
            return obj.gameObject.CompareTag(tag);
        }
    }

}
