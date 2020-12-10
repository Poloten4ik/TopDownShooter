﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;
    public GameObject shootPosition;

    public float fireRate = 1f;

    float nextFire;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
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

    private void Shoot()
    {
        animator.SetTrigger("Shoot");
        Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);
        nextFire = fireRate;
    }
}
