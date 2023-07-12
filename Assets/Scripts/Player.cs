using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] GameObject _playerBullet;

    private const float MoveSpeed = 8;
    private const float RollCooldown = 1;
    private const float RollDuration = 0.2f;
    private const float RollSpeed = 16;
    private const float BulletSpeed = 20;
    private const float ShootCooldown = 0.1f;

    private readonly KeyCode[] ShootDirections = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

    private Rigidbody2D _rigidbody;

    private Vector2 _moveDirection;

    private float _nextRoll;
    private bool _startRoll;
    private int _rollUpdatesRemaining;
    private Vector2 _rollDirection;

    private float _nextShoot;
    private bool _startShoot;
    private Vector2 _shootDirection;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _nextRoll = 0;
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
        _rigidbody.velocity = MoveSpeed * _moveDirection.normalized;

        if (_startRoll) {
            _rollUpdatesRemaining = (int)(RollDuration * 50);
            _startRoll = false;
        }
        if (_rollUpdatesRemaining > 0) {
            _rigidbody.velocity = RollSpeed * _rollDirection.normalized;
            _rollUpdatesRemaining--;
        }

        if (Time.time > _nextShoot)
            if (_shootDirection != Vector2.zero) {
                GameObject bullet = Instantiate(_playerBullet);
                // The z component ensures bullets appear behind the player
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                bullet.GetComponent<Rigidbody2D>().velocity = BulletSpeed * _shootDirection;
                _nextShoot = Time.time + ShootCooldown;
            }
    }
}
