using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckTheBrick
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        public uint damage { get; private set; } = 1;

        [SerializeField]
        private float _speedBase = 5;
        private float _speed;
        private float _speedCurrent;
        private Transform _transform;
        private Rigidbody2D _rigidbody;

        public event Action<Ball> OnDestroyed;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
            _speed = _speedBase;
            _speedCurrent = _speed;
        }

        // Start is called before the first frame update
        void Start()
        {
            //Invoke("Move", 2f);
        }

        void Move()
        {
            _rigidbody.velocity = (Vector2.one.normalized * _speedCurrent);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _speedCurrent;
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void SetDirection(Vector2 direction)
        {
            _rigidbody.velocity = direction * _speedCurrent;
        }

        public Vector2 GetDirection()
        {
            return _rigidbody.velocity.normalized;
        }

        public void Stick(Transform parent)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.isKinematic = true;
            _transform.parent = parent;
        }

        public void Release(Vector2 direction)
        {
            _rigidbody.isKinematic = false;
            _transform.parent = null;
            SetDirection(direction);
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 0) Destroy(gameObject);
        }

        public void AddDamage(uint amount)
        {
            damage += amount;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            Debug.Log("Not Implemented UpdateVisuals");
        }
    }
}