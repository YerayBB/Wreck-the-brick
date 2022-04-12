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

        [Header("PowerUp Fields")]
        [SerializeField]
        private GameObject _powerUpPrefab;
        [SerializeField]
        private PowerUpData[] _powerUpTypes;
        [SerializeField]
        private PowerUpData[] _powerUpRareTypes;
        [SerializeField]
        [Tooltip("from 0 to 100")]
        private int _powerUpRareChance = 15;

        [Header("Level Config")]
        [SerializeField]
        private Bounds _levelBounds = new Bounds(new Vector3(0, 2, 0), new Vector3(23, 13, 0));
        [SerializeField]
        [Tooltip("Columns")]
        private int levelSizex;
        [SerializeField]
        [Tooltip("Rows")]
        private int levelSizey;
        [SerializeField]
        private GameObject _brickPrefab;
        [SerializeField]
        private BrickData[] _brickTypes;

        [SerializeField]
        [Tooltip("Predefined Levels")]
        private Level[] _levels;

        private LevelBuilder _levelBuilder;
        private PoolMono<PowerUp> _powerUpPool;
        private List<Ball> _balls;
        private Controls _inputs;

        private Player _player;
        private int _lives = 3;


        #region MonoBehaviourCalls

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

                //LevelBuilder Config
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
                    _player.DisableInputs();
                    _inputs.MenuClear.Enable();
                };

                //Input Config
                _inputs = new Controls();
                _inputs.MenuGameOver.Disable();
                _inputs.MenuGameOver.Retry.performed += (context) =>
                {
                    //Reload scene
                    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                };
                _inputs.MenuClear.Start.performed += (context) =>
                {
                    //Generate next level and start it
                    _inputs.MenuClear.Disable();
                    UIManager.Instance.ContinueUI();
                    CreateLevel();
                    StartGame();
                };
                _inputs.MenuClear.Exit.performed += (context) => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                _inputs.MenuGameOver.Exit.performed += (context) => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }

        void Start()
        {
            UIManager.Instance.ShowLives(_lives);
            CreateLevel();
            StartGame();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_levelBounds.center, _levelBounds.size);
        }

        #endregion


        public void SetPlayer(Player p)
        {
            _player = p;
        }

        public void AddBall(int amount)
        {
            if (_balls.Count > 0)
            {
                for (int i = 0; i < amount; ++i)
                {
                    SpawnBall(_balls[0].transform.position).SetDirection((Quaternion.AngleAxis(Random.Range(-15, 15), Vector3.forward) * Vector3.up));
                }
            }
        }

        public void SpawnPowerUp(Vector3 pos)
        {
            if (Random.Range(0, 100) < _powerUpRareChance) _powerUpPool.GetItem().Initialize(pos, _powerUpRareTypes[Random.Range(0, _powerUpRareTypes.Length)]);
            else _powerUpPool.GetItem().Initialize(pos, _powerUpTypes[Random.Range(0, _powerUpTypes.Length)]);
        }

        public void AddPowerToBalls(int amount)
        {
            foreach (Ball ball in _balls)
            {
                ball.AddDamage((uint)amount);
            }
        }

        public void AddLives(int amount)
        {
            _lives += amount;
            UIManager.Instance.ShowLives(_lives);
            if (_lives <= 0) GameOver();
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

        private void GameOver()
        {
            _player.DisableInputs();
            _inputs.MenuGameOver.Enable();
            UIManager.Instance.GameOverUI();
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

        private Ball SpawnBall(Vector2 position)
        {
            Ball aux = Instantiate<Ball>(_ballPrefab, position, Quaternion.identity);
            aux.OnKilled +=
                (ball) =>
                {
                    _balls.Remove(ball);
                    if (_balls.Count == 0)
                    {
                        LoseLives(1);
                    }
                };
            _balls.Add(aux);
            return aux;
        }

    }
}