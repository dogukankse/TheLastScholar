using System.Collections;
using System.Collections.Generic;
using Base;
using Characters;
using Cinemachine;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Public

        public List<GameObject> ActiveBooks => _activeBooks;
        public float StartTime { get; private set; }
        public float PlayerFireRate => _playerFireRate;
        public float PlayerFireSpeed => _playerFireSpeed;
        public float EnemyFireRate => _enemyFireRate;

        public int MeleeAttackDamage => _meleeAttackDamage;

        public int RangeAttackDamage => _rangeAttackDamage;

        public WeightBar Bar => _bar;

        public bool IsGameEnded => _isGameEnded;

        #endregion

        #region Private

        [Header("Cameras")] [SerializeField] private CinemachineVirtualCamera _playerCam;
        [SerializeField] private CinemachineVirtualCamera _deadCam;

        [Header("UI")] [SerializeField] private WeightBar _bar;
        [SerializeField] private InfoFrame _infoFrame;

        [Header("Player")] [SerializeField] private float _playerFireSpeed;
        [SerializeField] private float _playerFireRate;

        [Header("World Settings")] [SerializeField]
        private GameObject _projectile;

        [SerializeField] private GameObject _enemyProjectile;
        [SerializeField] private GameObject _book;
        [SerializeField] private float _bookSpawnRate;


        [Header("Enemy")] [SerializeField] private GameObject _enemy;
        [SerializeField] private float _enemySpawnRate;
        [SerializeField] private int _meleeAttackDamage;
        [SerializeField] private int _rangeAttackDamage;
        [SerializeField] private float _enemyFireRate;


        private ObjectPool _bookPool;
        private ObjectPool _enemyPool;
        private List<GameObject> _activeBooks;
        private List<GameObject> _activeEnemies;
        private bool _isGameEnded;
        private readonly Vector2 _minMaxEnemySpawnDist = new Vector2(20, 30);
        private readonly Vector2 _minMaxBookSpawnDist = new Vector2(15, 25);

        #endregion

        public IEnumerator EndGame()
        {
            _isGameEnded = true;
            _playerCam.Priority = 9;
            _deadCam.Priority = 10;
            CancelInvoke(nameof(SpawnBooks));
            CancelInvoke(nameof(SpawnEnemy));
            foreach (var book in _activeBooks)
            {
                book.SetActive(false);
            }

            PushAllActiveEnemies();
            yield return new WaitForSeconds(2f);
            foreach (var enemy in _activeEnemies)
            {
                enemy.SetActive(false);
            }

            _activeBooks.Clear();
            _activeEnemies.Clear();


            _infoFrame.gameObject.SetActive(true);
            _infoFrame.SetData(Player.Instance.LifeTime, Player.Instance.KilledEnemyCount + "",
                Player.Instance.Wisdom.ToString("F"));
            Time.timeScale = 0;
        }

        private void Start()
        {
            _activeBooks = new List<GameObject>();
            _activeEnemies = new List<GameObject>();

            Player.Instance.KilledEnemyCount = 0;

            _playerCam.Priority = 10;
            _deadCam.Priority = 9;

            ObjectPool.CreatePool("projectile", 100, _projectile, new GameObject("Projectiles").transform);
            ObjectPool.CreatePool("enemyProjectile", 100, _enemyProjectile, new GameObject("EnemyProjectiles").transform);
            _bookPool = ObjectPool.CreatePool("book", 100, _book, new GameObject("Books").transform);
            _enemyPool = ObjectPool.CreatePool("enemy", 150, _enemy, new GameObject("Enemies").transform);
            StartTime = Time.time;

            AudioManager.Instance.PlayMusic(.2f);
            InvokeRepeating(nameof(SpawnBooks), .5f, _bookSpawnRate);
            InvokeRepeating(nameof(SpawnEnemy), 1.5f, _enemySpawnRate);
        }

        private void Update()
        {
            BookCheck();
        }

        private void FixedUpdate()
        {
            EnemyMoves();
        }

        private void SpawnBooks()
        {
            if (_activeBooks.Count > _bookPool.Count * .9f) return;
            GameObject go = _bookPool.GetPoolObject();
            float dist = Random.Range(_minMaxBookSpawnDist.x, _minMaxBookSpawnDist.y);
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            go.transform.position = Player.Instance.Position + dist * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            _activeBooks.Add(go);
        }

        private void BookCheck()
        {
            foreach (var book in _activeBooks)
            {
                if (Vector2.Distance(book.transform.position, Player.Instance.Position) > 50)
                {
                    book.SetActive(false);
                }
            }
        }

        private void SpawnEnemy()
        {
            if (_activeEnemies.Count > _enemyPool.Count * .9f) return;
            GameObject go = _enemyPool.GetPoolObject();
            go.GetComponent<Enemy>().Init();
            float dist = Random.Range(_minMaxEnemySpawnDist.x, _minMaxEnemySpawnDist.y);
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            go.transform.position = Player.Instance.Position + dist * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            _activeEnemies.Add(go);
        }

        private void EnemyMoves()
        {
            if (_isGameEnded) return;
            foreach (var enemy in _activeEnemies)
            {
                Enemy e = enemy.GetComponent<Enemy>();
                e.FollowPlayer();
            }
        }

        private void PushAllActiveEnemies()
        {
            foreach (var enemy in _activeEnemies)
            {
                Enemy e = enemy.GetComponent<Enemy>();
                e.PushBack();
            }
        }
    }
}