using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsUnknown;

namespace WreckTheBrick
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField]
        private Ball _ballPrefab;
        [SerializeField]
        private GameObject _powerUpPrefab;

        private int _lives;

        private PoolMono<PowerUp> _powerUpPool;
        private List<Ball> _balls;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                _balls = new List<Ball>();
                _powerUpPool = new PoolMono<PowerUp>(_powerUpPrefab);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //SpawnBall(Vector2.zero);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GameOver()
        {
            Debug.Log("GAMEOVER");
        }

        public Ball SpawnBall(Vector2 position)
        {
            Ball aux = Instantiate<Ball>(_ballPrefab, position, Quaternion.identity);
            aux.OnDestroyed +=
                (ball) =>
                {
                    _balls.Remove(ball);
                    if(_balls.Count == 0)
                    {
                        AddLives(-1);
                    }
                };
            _balls.Add(aux);
            return aux;
        }

        public void AddBall(int amount)
        {
            Debug.Log("Added ball");
        }

        public void AddLives(int amount)
        {
            _lives += amount;
            if (_lives <= 0) GameOver();
        }

        public void AddPowerToBalls(int amount)
        {
            foreach(Ball ball in _balls)
            {
                ball.AddDamage(amount);
            }
        }
    }
}