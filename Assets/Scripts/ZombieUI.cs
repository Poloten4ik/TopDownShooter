﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts 
{
    public class ZombieUI : MonoBehaviour
    {
        public Zombie zombie;
        public Slider healthSlider;

        void Start()
        {
            healthSlider.maxValue = zombie.health;
            healthSlider.value = zombie.health;

            zombie.HealthChanged += UpdateHealthBar;
        }

        public void UpdateHealthBar()
        {
            healthSlider.value = zombie.health;
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}

