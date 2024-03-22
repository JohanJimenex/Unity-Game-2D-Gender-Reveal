using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [Header("Player References")]
    [SerializeField] private PlayerHealthManager playerHealthManager;

    [Header("UI References")]

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject bestScoreUI;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private List<Image> hearts; // Lista de corazones
    [SerializeField] private Sprite fullHeart; // Sprite de corazón lleno
    [SerializeField] private Sprite emptyHeart; // Sprite de corazón vacío

    private int bestScore;

    private void Awake() {
        LoadBestScore();
    }

    private void Start() {
        SubscribeAndListenEvents();
    }
    
    private void Update() {
        KeepScoreUIUpdated();
    }

    private void SubscribeAndListenEvents() {
        // playerMovement.OnPositionYChanged += UpdateUIScore;
        playerHealthManager.OnPlayerGetDamage += DecreasesUIHearts;
        playerHealthManager.OnPlayerIncreaseLife += IncreasesUILifes;
        playerHealthManager.OnPlayerDied += HandlePlayerDied;
    }

    private void LoadBestScore() {
        bestScore = PlayerPrefs.GetInt("BestScore");
        UpdateBestScoreUI();
    }

    private void KeepScoreUIUpdated() {

        int score = GameManager.GetScore();
        scoreText.text = score.ToString("N0");

        if (score > bestScore) {
            bestScore = score;
            UpdateBestScoreUI();
        }
    }

    private void UpdateBestScoreUI() {
        bestScoreText.text = bestScore.ToString("N0");
    }

    private void DecreasesUIHearts(int lifes) {

        for (int i = 0; i < hearts.Count; i++) {
            if (i + 1 > lifes) {
                hearts[i].sprite = emptyHeart; // Corazón vacío
            }
            else {
                hearts[i].sprite = fullHeart; // Corazón lleno
            }
        }
    }

    private void IncreasesUILifes(int lifes) {

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
