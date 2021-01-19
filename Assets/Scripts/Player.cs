using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Lean.Pool;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public Action HealthChanged = delegate { };

        public float fireRate = 1f;
        public float health = 100;
        public float yourMoney = 0;
        public bool isPlayerAlive = true;

        public Bullet bulletPrefab;
        public Grenade grenadePrefab;
        public GameObject shootPosition;
        public GameObject grenadePosition;


        float nextFire;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetButton("Fire1") && nextFire <= 0)
            {
                Shoot();
            }

            if (nextFire > 0)
            {
                nextFire -= Time.deltaTime;
            }

            ThrowGrenade();
        }

        public void LoseHp(float damage)
        {
            health -= damage;
            HealthChanged();
            if (health <= 0)
            {
                GameOver();
                isPlayerAlive = false;
                gameObject.GetComponent<Collider2D>().isTrigger = true;
            }
        }

        public void AddHp(float hp)
        {
            health += hp;
        }
        
        public void AddMoney(float money)
        {
            yourMoney += money;
        }

        private void GameOver()
        {
            animator.SetTrigger("Death");
            StartCoroutine(Restart());
        }

        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(0);
        }

        private void Shoot()
        {
            if (isPlayerAlive)
            {
                animator.SetTrigger("Shoot");
                LeanPool.Spawn(bulletPrefab, shootPosition.transform.position, transform.rotation);
                nextFire = fireRate;
            }
        }

        private void ThrowGrenade()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                LeanPool.Spawn(grenadePrefab, grenadePosition.transform.position, transform.rotation);
            }
        }
    }
}
