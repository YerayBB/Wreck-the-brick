using UnityEngine;
using UtilsUnknown;

namespace WreckTheBrick
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PowerUp : PoolableBehaviour
    {
        [SerializeField]
        private float _speed;
        
        private Transform _transform;
        private SpriteRenderer _renderer;
        private Rigidbody2D _rigidbody;

        private string _name;
        private System.Action<Player> _powerAction;

        #region MonoBehaviourCalls

        private void Awake()
        {
            _transform = transform;
            _renderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_init)
            {
                Player player;
                if (collision.gameObject.TryGetComponent(out player))
                {
                    _powerAction?.Invoke(player);
                    UIManager.Instance.Notification(_name);
                }
                Disable();
            }
        } 

        #endregion

        public void Initialize(Vector3 position,PowerUpData data)
        {
            _init = false;
            _transform.position = position;
            _renderer.sprite = data.sprite;
            _renderer.color = data.color;
            _powerAction = data.ApplyEffect;
            _name = data.name;

            gameObject.SetActive(true);

            _rigidbody.velocity = Vector2.down * _speed;
            _init = true;
        }

    }
}