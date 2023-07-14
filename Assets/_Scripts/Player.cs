using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static int Score { get; set; }

    public PlayerData Data { get; private set; }
    public int CurrentHealth { get; private set; }

    private const float RollReadyFlashLength = 0.1f;

    private static readonly KeyCode[] ShootDirections = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

    [SerializeField] private GameObject _playerBullet;
    [SerializeField] private ScriptablePlayer _scriptable;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _moveDirection;

    private float _nextRoll;
    private bool _startRoll;
    private int _rollUpdatesRemaining;
    private Vector2 _rollDirection;

    private float _nextShoot;
    private bool _startShoot;
    private Vector2 _shootDirection;

    private float _lastDamage;

    private bool IsInvincible {
        get => (Time.time < _lastDamage + Data.TimeInvincibleAfterHit || _rollUpdatesRemaining > 0);
    }

    private void Start() {
        Data = _scriptable.Data;

        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        CurrentHealth = Data.MaxHealth;
        _nextRoll = -Data.RollCooldown;
        _lastDamage = -Data.TimeInvincibleAfterHit;
        _spriteRenderer.color = Data.DefaultColor;
    }

    private void Update() {
        _moveDirection.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        _moveDirection.y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);

        _shootDirection.x = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        _shootDirection.y = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextRoll && _moveDirection != Vector2.zero) {
            _startRoll = true;
            _rollDirection = _moveDirection;
            _nextRoll = Time.time + Data.RollCooldown;
        }

        _spriteRenderer.color = GetColor();
    }

    private void FixedUpdate() {
        // Weapon recoil is applied only to speed up the player. The dot product means firing
        // diagonally behind is less effective than firing directly behind
        _rigidbody.velocity = Data.MoveSpeed * _moveDirection.normalized -
            Mathf.Min(Data.WeaponRecoil * Vector2.Dot(_shootDirection.normalized, _moveDirection.normalized), 0) * _moveDirection.normalized;

        if (_startRoll) {
            // 50 is the number of FixedUpdates per second
            _rollUpdatesRemaining = (int)(Data.RollDuration * 50);
            _startRoll = false;
        }

        if (_rollUpdatesRemaining > 0) {
            _rigidbody.velocity = Data.RollSpeed * _rollDirection.normalized;
            _rollUpdatesRemaining--;
        }

        if (Time.time > _nextShoot) {
            if (_shootDirection != Vector2.zero) {
                GameObject bullet = Instantiate(_playerBullet);
                // The z component ensures bullets appear behind the player
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                bullet.GetComponent<Rigidbody2D>().velocity = Data.BulletSpeed * _shootDirection.normalized;
                _nextShoot = Time.time + Data.ShootInterval;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy" && !IsInvincible) {
            CurrentHealth -= other.gameObject.GetComponent<Enemy>().Data.Damage;
            _lastDamage = Time.time;
        }
    }

    private Color GetColor() {
        if (_rollUpdatesRemaining > 0)
            return Data.RollColor;
        if (IsInvincible)
            return Data.InvincibleColor;
        if (Time.time > _nextRoll && Time.time < _nextRoll + RollReadyFlashLength)
            return Data.RollReadyColor;
        return Data.DefaultColor;
    }
}
