using Base;
using Characters;
using Managers;
using UI;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite _deadSprite;

    [Header("Movement")] [SerializeField] [Range(1f, 20f)]
    private float _moveSpeed;

    [Header("Attack")] [SerializeField] private Transform _firePoint;
    [SerializeField] private WisdomUI _wisdom;


    private bool _canMove;
    private Rigidbody2D _rigidbody;
    private PlayerInput _input;

    public void TakeDamage(int damage)
    {
        GameManager.Instance.Bar.DecreaseValue(damage);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = ScriptableObject.CreateInstance<PlayerInput>();
        _input.Fire = Fire;
        _input.Absorb = Absorb;
        GameManager.Instance.Bar.Dead = Dead;
        Player.Instance.TakeDamage = TakeDamage;
    }


    private void Update()
    {
        if (GameManager.Instance.IsGameEnded) return;
        _moveSpeed = Mathf.Lerp(1f, 5f, (100 - GameManager.Instance.Bar.Weight) / 95);

        if (_input.AbsorbInput())
        {
            _canMove = false;
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        _canMove = true;
        _input.MovementInput();
        _input.MouseInput();
        _input.DashInput();

        Player.Instance.Position = transform.position;
    }


    private void FixedUpdate()
    {
        if (!_canMove) return;
        _rigidbody.velocity = _input.MovementVec * (_moveSpeed * Time.fixedDeltaTime * 100);

        _input.LookDir = _input.MousePos - _rigidbody.position;
        float angle = Mathf.Atan2(_input.LookDir.y, _input.LookDir.x) * Mathf.Rad2Deg - 90;
        _firePoint.eulerAngles = new Vector3(0, 0, angle);

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Book book))
        {
            AudioManager.Instance.PlayBookPickup();
            bool status = GameManager.Instance.Bar.IncreaseValue(book.Weight);
            book.gameObject.SetActive(!status);
            GameManager.Instance.ActiveBooks.Remove(book.gameObject);
        }
        else if (other.TryGetComponent(out EnemyProjectile projectile))
        {
            GameManager.Instance.Bar.DecreaseValue(projectile.Damage);
        }
    }


    private void Fire()
    {
        GameObject go = ObjectPool.Pools["projectile"].GetPoolObject();
        Projectile p = go.GetComponent<Projectile>();
        p.SetData(_firePoint, _input.LookDir, GameManager.Instance.PlayerFireSpeed);
        AudioManager.Instance.PlayPlayerAttack();
    }

    private void Absorb()
    {
        GameManager.Instance.Bar.Absorb();
        _wisdom.UpdateWisdom();
    }

    private void Dead()
    {
        AudioManager.Instance.PlayPlayerDead();
        _renderer.sprite = _deadSprite;
        StartCoroutine(GameManager.Instance.EndGame());
    }
}