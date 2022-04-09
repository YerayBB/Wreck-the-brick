using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckTheBrick
{
    [CreateAssetMenu(fileName = "new Brick", menuName = "Scriptable/Brick")]
    public class BrickData : ScriptableObject
    {
        public Sprite sprite => _sprite;
        public int maxHP => _maxHP;
        public bool inmune => _maxHP < 0;

        [SerializeField]
        private Sprite _sprite;
        [SerializeField]
        private int _maxHP;

    }
}