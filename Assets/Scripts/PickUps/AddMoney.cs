using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.AbstractPickUps
{
    public class AddMoney : AbsctractPickUp
    {
        public float numberOfMoney;
        public override void ApplyEffect()
        {
            Player player = FindObjectOfType<Player>();
            player.AddMoney(numberOfMoney);
        }
    }

}
