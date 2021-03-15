using System;
using System.Collections;
using Characters;
using Managers;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private BoxCollider2D _collider;
    
    private Vector3 _dir;
    private float _speed;

    public void SetData(Transform startPos, Vector2 dir, float speed)
    {
        transform.position = startPos.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        _rigidbody.rotation = angle;
        _speed = speed;

        _rigidbody.AddForce(startPos.up * _speed, ForceMode2D.Impulse);
        
        _renderer.enabled = true;
        _collider.enabled = true;

        StartCoroutine(DespawnProjectile());
    }

    private IEnumerator DespawnProjectile()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            AudioManager.Instance.PlayEnemyTakeDamage();
            enemy.Health -= Player.Instance.Wisdom;
            gameObject.SetActive(false);
        }
    }
}