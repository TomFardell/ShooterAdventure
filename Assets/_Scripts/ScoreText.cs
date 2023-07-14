using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    [SerializeField] private Text _text;

    private void Update() {
        _text.text = Player.Score.ToString();
    }
}
