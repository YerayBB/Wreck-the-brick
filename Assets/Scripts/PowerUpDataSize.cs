using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsUnknown.Extensions;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new PowerUp size", menuName = "Scriptable/Power Up/Size")]
    public class PowerUpDataSize : PowerUpData
    {
        public override void ApplyEffect(Player player)
        {
            player.transform.localScale += Vector3.right * _effectStrength;
            if (_duration > 0)
            {
                player.DelayedCall(() => player.transform.localScale += Vector3.right * -_effectStrength, _duration);
            }
        }
    }
}