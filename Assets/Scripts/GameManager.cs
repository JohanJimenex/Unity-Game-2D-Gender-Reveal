using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public PlayerController playerTransform;

    // private int score;
    private int bestScore;

    private void Awake() {
        LoadBestScore();
    }

    private void Start() {
        OnPlayerPositionYChanged();
    }

    private void OnPlayerPositionYChanged() {
        playerTransform.OnPositionYChanged += UpdateBestScore;
    }


    // private void UpdateScore(float positionY) {

    //     score = (int)positionY;

    //     if (score > bestScore) {
    //         UpdateBestScore();
    //     }
    // }

    private void UpdateBestScore(float playerPositionY) {

        if ((int)playerPositionY > bestScore) {
            PlayerPrefs.SetInt("BestScore", (int)playerPositionY);
        }
    }

    private void LoadBestScore() {
        bestScore = PlayerPrefs.GetInt("BestScore");
    }

    public void StartGame() {
        SceneManager.LoadScene("MainScene");
    }

}
