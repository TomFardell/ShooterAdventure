using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    [SerializeField] private Text _text;

    private Player _player;

    private void Start() {
        _player = FindObjectOfType<Player>();
    }

    private void Update() {
        _text.text = _player.Score.ToString();
    }
}
