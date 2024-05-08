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

    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private GameObject screenBorderDamage;

    [SerializeField] private TextMeshProUGUI extraScoreText;

    private int bestScore;

    private void Awake() {
        LoadBestScore();
    }

    private void Start() {
        SubscribeAndListenEvents();
        oxygenSlider.maxValue = playerHealthManager.MaxLifes;
    }

    private void Update() {
        KeepScoreUIUpdated();
    }

    private void SubscribeAndListenEvents() {
        // playerMovement.OnPositionYChanged += UpdateUIScore;
        playerHealthManager.OnPlayerGetDamage += DecreasesUIlifes;
        playerHealthManager.OnPlayerGetDamage += ShowScreenBorderIndicator;
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

    private void DecreasesUIlifes(int lifes) {
        oxygenSlider.value -= lifes;
    }

    private void IncreasesUILifes(int lifesPointsToIncrease) {
        oxygenSlider.value += lifesPointsToIncrease;
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

    private void ShowScreenBorderIndicator(int _) {
        screenBorderDamage.SetActive(true);
        Invoke(nameof(DisableScreenBorderIndicator), 0.5f);
    }

    private void DisableScreenBorderIndicator() {
        screenBorderDamage.SetActive(false);
    }

    public void ShowExtraScore(int extraScore) {
        extraScoreText.text = $"+{extraScore}";
        extraScoreText.gameObject.SetActive(true);
        Invoke(nameof(HideExtraScore), 1.5f);
    }

    private void HideExtraScore() {
        extraScoreText.gameObject.SetActive(false);
    }
}
