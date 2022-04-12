//using System;
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
        private LevelBuilder _levelBuilder;

        private int _lives = 3;
        private Player _player;

        private PoolMono<PowerUp> _powerUpPool;
        private List<Ball> _balls;

        private Controls _inputs;

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
                _levelBuilder = new LevelBuilder(_levelBounds, _brickPrefab);
                _levelBuilder.OnLevelComplete += () =>
                {
                    UIManager.Instance.ClearLevelUI();
                    foreach(Ball ball in _balls)
                    {
                        ball.enabled = false;
                        Destroy(ball.gameObject);
                    }
                    _balls.Clear();
                    _inputs.MenuClear.Enable();
                };

                _inputs = new Controls();
                _inputs.MenuGameOver.Disable();
                _inputs.MenuGameOver.Retry.performed += (context) =>
                {
                    Debug.Log("Called Retry");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                };
                _inputs.MenuClear.Start.performed += (context) =>
                {
                    _inputs.MenuClear.Disable();
                    UIManager.Instance.ContinueUI();
                    CreateLevel();
                    StartGame();
                };
                _inputs.MenuClear.Exit.performed += (context) => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                _inputs.MenuGameOver.Exit.performed += (context) => UnityEngine.SceneManagement.SceneManager.LoadScene(0);

            }
        }

        // Start is called before the first frame update
        void Start()
        {
            UIManager.Instance.ShowLives(_lives);
            CreateLevel();
            StartGame();
        }

        private void CreateLevel()
        {
            _levelBuilder.BuildLevel(new Level(6, 15, _brickTypes.Length), _brickTypes);
            
        }

        private void StartGame()
        {
            _player.AttachBall(SpawnBall(_player.transform.position + Vector3.up));
            _player.EnableInputs();
        }

        public void SetPlayer(Player p)
        {
            _player = p;
        }

        private void GameOver()
        {
            _player.DisableInputs();
            _inputs.MenuGameOver.Enable();
            UIManager.Instance.GameOverUI();
        }

        public Ball SpawnBall(Vector2 position)
        {
            Ball aux = Instantiate<Ball>(_ballPrefab, position, Quaternion.identity);
            aux.OnKilled +=
                (ball) =>
                {
                    _balls.Remove(ball);
                    if(_balls.Count == 0)
                    {
                        LoseLives(1);
                    }
                };
            _balls.Add(aux);
            return aux;
        }

        public void AddBall(int amount)
        {
            for(int i = 0; i< amount; ++i)
            {
                SpawnBall(_balls[0].transform.position).SetDirection((Quaternion.AngleAxis(Random.Range(-15,15), Vector3.forward) * Vector3.up));
            }
        }

        public void AddLives(int amount)
        {
            _lives += amount;
            UIManager.Instance.ShowLives(_lives);
            if (_lives <= 0) GameOver();
        }

        private void LoseLives(int amount)
        {
            _lives -= amount;
            UIManager.Instance.ShowLives(_lives);
            if (_lives <= 0) GameOver();
            else
            {
                StartGame();
            }
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
            if(Random.Range(0,10) > 9) _powerUpPool.GetItem().Initialize(pos, _powerUpRareTypes[Random.Range(0,_powerUpRareTypes.Length)]);
            else _powerUpPool.GetItem().Initialize(pos, _powerUpTypes[Random.Range(0, _powerUpTypes.Length)]);

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_levelBounds.center, _levelBounds.size);
        }
    }
}