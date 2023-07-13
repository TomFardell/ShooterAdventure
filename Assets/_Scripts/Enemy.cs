using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private const float MoveSpeed = 2;

    private Rigidbody2D _rigidbody;
    private GameObject _player;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate() {
        _rigidbody.velocity = MoveSpeed * (_player.transform.position - transform.position).normalized;
        transform.up = _player.transform.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "PlayerBullet") {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}
