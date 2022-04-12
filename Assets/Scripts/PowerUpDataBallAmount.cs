using UnityEngine;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new PowerUp ball amount", menuName = "Scriptable/Power Up/Ball amount")]
    public class PowerUpDataBallAmount : PowerUpData
    {
        public override void ApplyEffect(Player player)
        {
            int amount = (int)_effectStrength;
            if(amount > 0)
            { 
                GameManager.Instance.AddBall((int)amount);
            }
        }
    }
}