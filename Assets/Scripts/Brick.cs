using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsUnknown;

namespace WreckTheBrick
{
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class Brick : PoolableBehaviour
    {
        private SpriteRenderer _renderer;
        private Transform _transform;

        private bool _inmmune = false;
        private int _hp = 0;

        public System.Action<int> OnHpLost;


        private void Awake()
        {
            _transform = transform;
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(Vector3 pos, Vector3 size, BrickData data)
        {
            _init = false;
            _inmmune = data.inmune;
            _hp = data.maxHP;
            _renderer.sprite = data.sprite;
            _transform.position = pos;
            _transform.localScale = size;
            gameObject.SetActive(true);
            _init = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_init)
            {
                if (!_inmmune)
                {
                    Ball ball;
                    if (collision.gameObject.TryGetComponent<Ball>(out ball))
                    {
                        int amount = Mathf.Min((int) ball.damage, _hp);
                        _hp -= amount;
                        OnHpLost?.Invoke(amount);
                        if (_hp <= 0) Disable();
                    }
                }
            }
        }
    }
}