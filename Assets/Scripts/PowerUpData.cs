using UnityEngine;

namespace WreckTheBrick
{
    public abstract class PowerUpData : ScriptableObject
    {
        public Sprite sprite => _sprite;
        public Color color => _color;

        [SerializeField]
        protected Sprite _sprite;
        [SerializeField]
        protected float _effectStrength;
        [SerializeField]
        protected Color _color = Color.white;

        public abstract void ApplyEffect(Player player);
    }
}