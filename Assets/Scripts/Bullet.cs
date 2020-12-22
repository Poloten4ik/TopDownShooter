﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Items;

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

        private void Start()
        {
            rb.velocity = -transform.up * speed;
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(gameObject);  
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
