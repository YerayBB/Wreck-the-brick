using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new PowerUp ball amount", menuName = "Scriptable/Power Up/Ball amount")]
    public class PowerUpDataBallAmount : PowerUpData
    {
        public override void ApplyEffect(Player player)
        {
            int amount = (int)_effectStrength;
            if(amount > 1)
            {
                player.AttachBall(GameManager.Instance.SpawnBall(player.transform.position + Vector3.up));
                amount -= 1;
            }
            if (amount != 0)
            {
                GameManager.Instance.AddBall((int)amount);
            }
        }
    }
}