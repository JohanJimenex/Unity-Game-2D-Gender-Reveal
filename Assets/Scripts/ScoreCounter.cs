using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour {

    public Transform playerTransform;

    private TextMeshProUGUI scoreText;
    
    void Start() {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        scoreText.text = playerTransform.position.y.ToString("N0");
        if (int.Parse(scoreText.text) > PlayerPrefs.GetInt("BestScore")) {
            PlayerPrefs.SetInt("BestScore", int.Parse(scoreText.text));
        }
    }
}
