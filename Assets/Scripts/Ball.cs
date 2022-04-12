using UnityEngine;

namespace WreckTheBrick
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Ball : MonoBehaviour
    {
        public uint damage { get; private set; } = 1;

        [Header("Stats")]
        [SerializeField]
        private float _speed = 5;
        [SerializeField]
        private int _maxDammage = 6;
        
        [Header("Visuals")]
        [SerializeField]
        private Sprite _baseSprite;
        [SerializeField]
        private Sprite _upgradedSprite;
        [SerializeField]
        private Gradient _color;

        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;

        public event System.Action<Ball> OnKilled;

        #region MonoBehaviourCalls

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            UpdateVisuals();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 0) Kill();
            if (_rigidbody.velocity.normalized.x == 1) _rigidbody.velocity += Vector2.up * _speed / 2;
        }

        #endregion


        public Vector2 GetDirection()
        {
            return _rigidbody.velocity.normalized;
        }

        public void SetDirection(Vector2 direction)
        {
            _rigidbody.velocity = direction * _speed;
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

        public void AddDamage(uint amount)
        {
            damage += amount;
            Mathf.Clamp(damage, 1, _maxDammage);
            UpdateVisuals();
        }

        public void Kill()
        {
            OnKilled?.Invoke(this);
            Destroy(gameObject);
        }

        private void UpdateVisuals()
        {
            float ratio = (float)damage / (float)_maxDammage;
            _renderer.sprite = ratio >= 0.5 ? _upgradedSprite : _baseSprite;
            _renderer.color = _color.Evaluate(ratio);
        }
    }
}