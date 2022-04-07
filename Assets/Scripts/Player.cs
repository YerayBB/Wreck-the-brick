using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsUnknown.Extensions;
using UnityEngine.InputSystem;

namespace WreckTheBrick
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 10;
        [SerializeField]
        private float _maxBounceAngle = 75f;

        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private Controls _inputs;

        private bool _leftMove = false;
        private bool _rightMove = false;
        private uint _stickiness = 0;

        private Ball _attachedBall = null;
        private Coroutine _releaseBallTimer;





        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _transform = transform;

            _inputs = new Controls();
            _inputs.Player.Left.performed += (context) => _leftMove = true;
            _inputs.Player.Left.canceled += (context) => _leftMove = false;
            _inputs.Player.Right.performed += (context) => _rightMove = true;
            _inputs.Player.Right.canceled += (context) => _rightMove = false;
            _inputs.Player.Shot.performed += (context) => ReleaseBall();
            _inputs.Player.Enable();

        }

        private void ReleaseBall()
        {
            if(_attachedBall != null)
            {
                if (_releaseBallTimer != null) StopCoroutine(_releaseBallTimer);
                _attachedBall.Release(_transform.up);
                _attachedBall = null;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            AttachBall(GameManager.Instance.SpawnBall(_transform.position + Vector3.up));
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (_leftMove)
            {
                _rigidbody.AddForce(Vector2.left * _speed);
            }
            if (_rightMove)
            {
                _rigidbody.AddForce(Vector2.right * _speed);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Ball ball;
            if(collision.gameObject.TryGetComponent<Ball>(out ball))
            {
                if (_stickiness == 0)
                {
                    float offset = _transform.position.x - collision.GetContact(0).point.x;
                    float width = collision.otherCollider.bounds.size.x / 2;

                    float entranceAngle = Vector2.SignedAngle(_transform.up, ball.GetDirection());
                    float bounceAngle = offset / width * _maxBounceAngle;
                    bounceAngle = Mathf.Clamp(bounceAngle + entranceAngle, -_maxBounceAngle, _maxBounceAngle);

                    Quaternion rotation = Quaternion.AngleAxis(bounceAngle, Vector3.forward);

                    ball.SetDirection(rotation * _transform.up);
                }
                else
                {
                    AttachBall(ball);
                    _releaseBallTimer = this.DelayedCall(ReleaseBall, _stickiness * 5);
                }
            }
        }

        public void AttachBall(Ball ball)
        {
            if (_attachedBall != null) ReleaseBall();
            _attachedBall = ball;
            _attachedBall.Stick(_transform);
        }
    }
}