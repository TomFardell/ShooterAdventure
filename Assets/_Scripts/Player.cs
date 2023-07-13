using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private GameObject _playerBullet;
    [SerializeField] private Color RollColor;
    [SerializeField] private Color RollReadyColor;

    private const float MoveSpeed = 8;
    private const float RollCooldown = 1;
    private const float RollDuration = 0.2f;
    private const float RollSpeed = 16;
    private const float BulletSpeed = 25;
    private const float ShootInterval = 0.05f;
    private const float WeaponRecoil = 2;
    private const float RollReadyFlashLength = 0.1f;

    private readonly KeyCode[] ShootDirections = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Color _defaultColor;

    private Vector2 _moveDirection;

    private float _nextRoll;
    private bool _startRoll;
    private int _rollUpdatesRemaining;
    private Vector2 _rollDirection;

    private float _nextShoot;
    private bool _startShoot;
    private Vector2 _shootDirection;

    public int Score { get; set; }

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _defaultColor = _spriteRenderer.color;
        _nextRoll = -RollCooldown;
    }

    private void Update() {
        _moveDirection.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        _moveDirection.y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);

        _shootDirection.x = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        _shootDirection.y = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextRoll && _moveDirection != Vector2.zero) {
            _startRoll = true;
            _rollDirection = _moveDirection;
            _nextRoll = Time.time + RollCooldown;
        }
    }

    private void FixedUpdate() {
        // Weapon recoil is applied only to speed up the player. The dot product means firing
        // diagonally behind is less effective than firing directly behind
        _rigidbody.velocity = MoveSpeed * _moveDirection.normalized -
            Mathf.Min(WeaponRecoil * Vector2.Dot(_shootDirection.normalized, _moveDirection.normalized), 0) * _moveDirection.normalized;

        if (_startRoll) {
            // 50 is the number of FixedUpdates per second
            _rollUpdatesRemaining = (int)(RollDuration * 50);
            _spriteRenderer.color = RollColor;
            _startRoll = false;
        }

        if (_rollUpdatesRemaining > 0) {
            _rigidbody.velocity = RollSpeed * _rollDirection.normalized;
            _rollUpdatesRemaining--;
        } else {
            if (Time.time > _nextRoll && Time.time < _nextRoll + RollReadyFlashLength) {
                _spriteRenderer.color = RollReadyColor;
            } else {
                _spriteRenderer.color = _defaultColor;
            }
        }

        if (Time.time > _nextShoot)
            if (_shootDirection != Vector2.zero) {
                GameObject bullet = Instantiate(_playerBullet);
                // The z component ensures bullets appear behind the player
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * _shootDirection.normalized;
                _nextShoot = Time.time + ShootInterval;
            }
    }
}
