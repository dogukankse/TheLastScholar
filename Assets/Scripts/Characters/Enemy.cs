using System;
using Base;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
    public class Enemy : MonoBehaviour
    {
        private enum EnemyType
        {
            Melee,
            Ranged
        }

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health <= 0)
                {
                    Player.Instance.KilledEnemyCount++;
                    gameObject.SetActive(false);
                }
            }
        }

        [SerializeField] private Transform _firePoint;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite _meleeSprite;
        [SerializeField] private Sprite _rangedSprite;
        [Space] [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _meleeAttackDist;
        [SerializeField] private float _rangeAttackDist;

        private float _health;
        private EnemyType _type;
        private float _lastAttack;
        private int _damage;
        private Vector2 _lookDir;
        private bool _isInRange;

        public void Init()
        {
            _type = Random.value > .5f ? EnemyType.Melee : EnemyType.Ranged;
            switch (_type)
            {
                case EnemyType.Melee:
                    _renderer.sprite = _meleeSprite;
                    _health = (Time.time - GameManager.Instance.StartTime) * 1.05f;
                    break;
                case EnemyType.Ranged:
                    _renderer.sprite = _rangedSprite;
                    _health = (Time.time - GameManager.Instance.StartTime) * 1.01f;
                    break;
            }

            //Debug.Log(_health);
        }

        public void FollowPlayer()
        {
            if (_isInRange)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }

            Vector2 playerPos = Player.Instance.Position;
            _lookDir = playerPos - _rigidbody.position;

            _rigidbody.velocity = _lookDir.normalized * (_moveSpeed * Time.fixedDeltaTime * 100);
        }

        public void PushBack()
        {
            Vector2 playerPos = Player.Instance.Position;
            _lookDir = playerPos - _rigidbody.position;
            _rigidbody.AddForce(-_lookDir.normalized * (_moveSpeed * Time.fixedDeltaTime * 500), ForceMode2D.Impulse);
            //_rigidbody.velocity = _lookDir.normalized * (_moveSpeed * Time.fixedDeltaTime * 100);
        }

        private void Update()
        {
            switch (_type)
            {
                case EnemyType.Melee:
                    _damage = GameManager.Instance.MeleeAttackDamage;
                    if (Vector2.Distance(Player.Instance.Position, transform.position) <= _meleeAttackDist)
                        Attack();
                    else _isInRange = false;
                    break;
                case EnemyType.Ranged:
                    _damage = GameManager.Instance.RangeAttackDamage;
                    if (Vector2.Distance(Player.Instance.Position, transform.position) <= _rangeAttackDist)
                        Attack();
                    else _isInRange = false;
                    break;
            }
        }


        private void FixedUpdate()
        {
            float angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg - 90;
            _firePoint.eulerAngles = new Vector3(0, 0, angle);
        }

        private void Attack()
        {
            _isInRange = true;
            float deltaClickTime = Time.time - _lastAttack;
            if (deltaClickTime > GameManager.Instance.EnemyFireRate)
            {
                switch (_type)
                {
                    case EnemyType.Melee:
                        Player.Instance.TakeDamage(_damage);
                        AudioManager.Instance.PlayEnemyMeleeAttack();
                        break;
                    case EnemyType.Ranged:
                        EnemyProjectile projectile = ObjectPool.Pools["enemyProjectile"].GetPoolObject()
                            .GetComponent<EnemyProjectile>();
                        projectile.SetData(_firePoint, _lookDir, _damage, 30f);
                        AudioManager.Instance.PlayEnemyRangeAttack();

                        break;
                }

                _lastAttack = Time.time;
            }
        }
    }
}