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

        [SerializeField]
        private PowerUpData[] _powerUpTypes;
        [SerializeField]
        private PowerUpData[] _powerUpRareTypes;

        [SerializeField]
        private GameObject _brickPrefab;
        [SerializeField]
        private BrickData[] _brickTypes;
        [SerializeField]
        private Bounds _levelBounds = new Bounds(new Vector3(0, 2, 0), new Vector3(23, 13, 0));

        [SerializeField]
        private int levelSizex;
        [SerializeField]
        private int levelSizey;

        [SerializeField]
        private Level[] _levels;

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
            LevelBuilder level = new LevelBuilder(_levelBounds, _brickPrefab);
            level.BuildLevel(new Level(6,15,_brickTypes.Length), _brickTypes);
            level.OnLevelComplete += () => Debug.Log("Level Done");
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
                ball.AddDamage((uint)amount);
            }
        }

        public void SpawnPowerUp(Vector3 pos)
        {
            _powerUpPool.GetItem().Initialize(pos, _powerUpTypes[0]);
        }
        //23 13 0 /// 0 2 0

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(_levelBounds.center, _levelBounds.size);
        }
    }
}