using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsUnknown.Extensions;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new PowerUp sticky", menuName = "Scriptable/Power Up/Sticky")]
    public class PowerUpDataSticky : PowerUpData
    {
        public override void ApplyEffect(Player player)
        {
            player.AddStickiness((int)_effectStrength);
            player.DelayedCall(()=>player.AddStickiness((int)-_effectStrength), _duration);
        }
    }
}