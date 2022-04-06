using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckTheBrick
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 10;

        private Rigidbody2D _rigidbody;
        private Controls _inputs;

        private bool _leftMove = false;
        private bool _rightMove = false;



        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _inputs = new Controls();
            _inputs.Player.Left.performed += (context) => _leftMove = true;
            _inputs.Player.Left.canceled += (context) => _leftMove = false;
            _inputs.Player.Right.performed += (context) => _rightMove = true;
            _inputs.Player.Right.canceled += (context) => _rightMove = false;
            _inputs.Player.Enable();

        }

        // Start is called before the first frame update
        void Start()
        {

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
    }
}