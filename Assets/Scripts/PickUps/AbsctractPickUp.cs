using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AbstractPickUps
{
    public abstract class AbsctractPickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ApplyEffect();
                Destroy(gameObject);
            }
        }

        public abstract void ApplyEffect();
    }

}
