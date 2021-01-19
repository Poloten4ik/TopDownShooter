using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace Assets.Scripts
{
    public class AutoDestroyer : MonoBehaviour
    {
        public float destroyDelay = 1f;

        void Start()
        {
            LeanPool.Despawn(gameObject, destroyDelay);
        }

        void Update()
        {

        }
    }

}
