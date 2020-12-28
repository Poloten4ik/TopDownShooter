using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AbstractPickUps 
{
    public class AddLife : AbsctractPickUp
    {
        public float addLife;

        public override void ApplyEffect()
        {
            Player player = FindObjectOfType<Player>();
            player.AddHp(addLife);
        }
    }
}

