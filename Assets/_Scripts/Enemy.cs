using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public EnemyData Data { get; private set; }

    [SerializeField] private ScriptableEnemy _scriptable;

    private const float HitFlashLength = 0.05f;

    private Rigidbody2D _rigidbody;
    private Player _player;
    private UnitManager _unitManager;
    private SpriteRenderer _spriteRenderer;

    private int _currentHealth;
    private float _lastHit;

    private void Start() {
        Data = _scriptable.Data;

        _rigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<Player>();
        _unitManager = FindObjectOfType<UnitManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.color = Data.DefaultColor;
        _currentHealth = Data.MaxHealth;
    }

    private void Update() {
        _spriteRenderer.color = GetColor();
    }

    private void FixedUpdate() {
        _rigidbody.velocity = Data.MoveSpeed * (_player.transform.position - transform.position).normalized;
        transform.up = _player.transform.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "PlayerBullet") {
            _lastHit = Time.time;
            _currentHealth -= _player.Data.BulletDamage;

            if (_currentHealth <= 0) {
                Destroy(this.gameObject);
                _unitManager.DecrementEnemyCount();
                Player.Score++;
            }

            Destroy(other.gameObject);
        }
    }

    private Color GetColor() {
        if (Time.time > _lastHit && Time.time < _lastHit + HitFlashLength)
            return Data.HitColor;
        return Data.DefaultColor;
    }
}
