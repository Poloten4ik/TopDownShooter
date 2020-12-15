using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    private bool isEnemyAlive = true;

    public float enemyHealth;
    public float enemyFireDuration;

    public Text enemyHealthText;
    public Bullet enemyBulletPrefab;
    public GameObject shootPosition;
    public GameObject prefabPickUp;

    Player player;
    Animator animatorEnemy;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        animatorEnemy = GetComponent<Animator>();
    }

    private void Start()
    { 
        enemyHealthText.text = enemyHealth.ToString();
        StartCoroutine(Shoot());
    }
    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (isEnemyAlive)
        {
            Vector3 enemyPosition = transform.position;
            Vector3 direction = player.transform.position - enemyPosition;
            direction.z = 0;
            transform.up = -direction;
        }
        else
        {
            transform.up = transform.position;
        }
       
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyFireDuration);
            animatorEnemy.SetTrigger("ShootEnemy");
            Instantiate(enemyBulletPrefab, shootPosition.transform.position, transform.rotation);
        }
    }

    public void LoseHp(float damage)
    {
        enemyHealth -= damage;
        enemyHealthText.text = enemyHealth.ToString();

        if (enemyHealth <= 0)
        {
            animatorEnemy.SetTrigger("DeathEnemy");

            enemyHealthText.enabled = false;
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            isEnemyAlive = false;

            Instantiate(prefabPickUp, transform.position, Quaternion.identity);
            StopAllCoroutines();
        }
    }
}

