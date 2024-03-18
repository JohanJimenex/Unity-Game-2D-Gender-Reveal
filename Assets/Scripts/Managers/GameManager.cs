using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public PlayerMovement playerMovement;

    private int bestScore;

    private void Awake() {
        LoadBestScore();
    }

    private void Start() {
        playerMovement.OnPositionYChanged += UpdateBestScore;
    }

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
