using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MainUi : MonoBehaviour
    {
        public Slider playerHealth;
        public Player player;

        public void Start()
        {
            player = FindObjectOfType<Player>();
            player.HealthChanged += UpdateHealth;

            playerHealth.maxValue = player.health;
            playerHealth.value = player.health;
        }

        public void UpdateHealth()
        {
            playerHealth.value = player.health;
        }
    }

}
