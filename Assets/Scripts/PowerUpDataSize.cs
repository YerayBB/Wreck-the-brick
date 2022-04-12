using UnityEngine;
using UtilsUnknown.Extensions;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new PowerUp size", menuName = "Scriptable/Power Up/Size")]
    public class PowerUpDataSize : PowerUpData
    {
        [SerializeField]
        protected float _duration = 0;

        public override void ApplyEffect(Player player)
        {
            player.ChangeSize(_effectStrength);
            if (_duration > 0)
            {
                player.DelayedCall(() => player.ChangeSize(-_effectStrength), _duration);
            }
        }
    }
}