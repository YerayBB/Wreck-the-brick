using UnityEngine;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new PowerUp lives", menuName = "Scriptable/Power Up/Lives")]
    public class PowerUpDataLives : PowerUpData
    {
        public override void ApplyEffect(Player player)
        {
            GameManager.Instance.AddLives((int)_effectStrength);
        }

    }
}