using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PickUp : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Player player = FindObjectOfType<Player>();


            if (collision.gameObject.CompareTag("Player"))
            {
                player.AddHp();
                Destroy(gameObject);
            }
        }
    }

}
