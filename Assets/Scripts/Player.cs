using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float fireRate = 1f;
    public float health;
    public bool isPlayerAlive = true;

    public Bullet bulletPrefab;
    public GameObject shootPosition;
    public Text healthText;

    float nextFire;

    Animator animator;
    Screen screen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        screen = FindObjectOfType<Screen>();
    }

    void Start()
    {
        healthText.text = health.ToString();
    }

    // Update is called once per frame
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
    }

    public void LoseHp(float damage)
    {
        if (health <= 0)
        {
            GameOver();
            healthText.enabled = false;
            isPlayerAlive = false;
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            screen.ScreenBlackOut();
        }

        {
            health -= damage;
            healthText.text = health.ToString();
        }

    }

    public void AddHp()
    {
        health += 10;
        healthText.text = health.ToString();
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
            Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);
            nextFire = fireRate;
        }
        else
        {
            // ничего не делать
        }
      
    }
}
