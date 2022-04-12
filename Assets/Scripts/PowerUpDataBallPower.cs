using UnityEngine;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new PowerUp ball power", menuName = "Scriptable/Power Up/Ball power")]
    public class PowerUpDataBallPower : PowerUpData
    {
        public override void ApplyEffect(Player player)
        {
            GameManager.Instance.AddPowerToBalls((int)_effectStrength);
        }
    }
}