using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private PlayerController playerController;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject bestScoreUI;

    [SerializeField] private List<Image> hearts; // Lista de corazones
    [SerializeField] private Sprite fullHeart; // Sprite de corazón lleno
    [SerializeField] private Sprite emptyHeart; // Sprite de corazón vacío


    private void Awake() {
        LoadBestScore();
    }

    private void Start() {
        playerController.OnPositionYChanged += UpdateUIScore;
        playerController.OnHurt += UpdateHearts;
        playerController.OnPlayerDied += HandlePlayerDied;
    }

    private void LoadBestScore() {
        bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString("N0");
    }

    private void UpdateUIScore(float playerPositionY) {
        scoreText.text = playerPositionY.ToString("N0");
    }

    void UpdateHearts(int health) {
        for (int i = 0; i < hearts.Count; i++) {
            if (i < health) {
                hearts[i].sprite = fullHeart; // Corazón lleno
            }
            else {
                hearts[i].sprite = emptyHeart; // Corazón vacío
            }
        }
    }


    private void AddLive() {

        // Encuentra el primer corazón vacío y cámbialo a un corazón lleno
        for (int i = 0; i < hearts.Count; i++) {
            if (hearts[i].sprite == emptyHeart) {
                hearts[i].sprite = fullHeart;
                break;
            }
        }
    }

    private void HandlePlayerDied() {
        Invoke(nameof(ShowGameOverPanel), 1.2f);
        Invoke(nameof(ShowBestScorePanel), 1.2f);
    }

    private void ShowGameOverPanel() {
        gameOverPanel.SetActive(true);
    }

    private void ShowBestScorePanel() {
        bestScoreUI.SetActive(true);
    }
}
