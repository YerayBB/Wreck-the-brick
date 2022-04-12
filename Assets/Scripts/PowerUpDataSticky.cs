using UnityEngine;
using UtilsUnknown.Extensions;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new PowerUp sticky", menuName = "Scriptable/Power Up/Sticky")]
    public class PowerUpDataSticky : PowerUpData
    {
        [SerializeField]
        protected float _duration = 0;

        public override void ApplyEffect(Player player)
        {
            player.AddStickiness((int)_effectStrength);
            if (_duration > 0)
            {
                player.DelayedCall(() => player.AddStickiness((int)-_effectStrength), _duration);
            }
        }
    }
}