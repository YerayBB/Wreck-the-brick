using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckTheBrick
{
    public abstract class PowerUpData : ScriptableObject
    {
        public Sprite sprite => _sprite;

        [SerializeField]
        protected Sprite _sprite;
        [SerializeField]
        protected float _effectStrength;

        public abstract void ApplyEffect(Player player);
    }
}