using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestScore : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    private TextMeshProUGUI bestScoreText;

    void Start() {
        bestScoreText = GetComponent<TextMeshProUGUI>();
        bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
    }

}
