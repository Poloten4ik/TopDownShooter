using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class NextLevel : MonoBehaviour
    {
        public Transform checkPoint;
        private Player player;

        void Start()
        {
            player = FindObjectOfType<Player>();
        }
        
        void Update()
        {
            if (Vector2.Distance(player.transform.position,checkPoint.position) < 3f)
            {
                SceneManager.LoadScene(1);
            }
        }
    }

}

