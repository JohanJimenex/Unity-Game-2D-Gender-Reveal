using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [Header("GameObjects References")]
    [SerializeField] private PlayerHealthManager playerHealthManager;
    [SerializeField] private FirebaseConnection firebaseConnection;

    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject bestScoreUI;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private GameObject screenBorderDamage;

    [SerializeField] private TextMeshProUGUI extraScoreText;
    [SerializeField] private GameObject loadingPanel;

    [Header("Leaderboard UI")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TextMeshProUGUI leadersNameUI;
    [SerializeField] private TextMeshProUGUI leaderScoreUI;
    [SerializeField] private GameObject setBestScorePanel;
    [SerializeField] private TMP_InputField nameInput;

    private int bestScore;
    private List<Leaderboard> leaderboard;

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
        playerHealthManager.OnPlayerDied += OnPlayerDied;
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

    private async void OnPlayerDied() {

        leaderboard = await firebaseConnection.GetLeaderboardFromDB();

        foreach (Leaderboard leader in leaderboard) {
            if (PlayerPrefs.GetInt("BestScore") > leader.score) {
                setBestScorePanel.SetActive(true);
                break;
            }
        }

        Invoke(nameof(ShowGameOverPanel), 1.2f);
        Invoke(nameof(ShowBestScoreText), 1.2f);
    }

    private void ShowGameOverPanel() {
        gameOverPanel.SetActive(true);
    }

    private void ShowBestScoreText() {
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

    public void GetInputText() {
        string inputText = nameInput.text;
        int score = PlayerPrefs.GetInt("BestScore");
        firebaseConnection.WriteNewLeaderOnDB(inputText, score);
        GetLeaderboard();
    }

    private void WriteNewLeader(String name, int score) {
        firebaseConnection.WriteNewLeaderOnDB(name, score);
        GetLeaderboard();
    }

    public async void GetLeaderboard() {
        loadingPanel.SetActive(true);
        leaderboard = await firebaseConnection.GetLeaderboardFromDB();
        UpdateLeaderboardUI();
    }

    private void UpdateLeaderboardUI() {

        int counter = 1;

        foreach (Leaderboard leader in leaderboard) {

            if (leader.name.Length > 13) {
                leader.name = leader.name.Substring(0, 13);
            }

            leadersNameUI.text += $"{counter}  {leader.name.ToUpper()} \n";

            if (leader.score > 999999) {
                leader.score = 999999;
            }

            leaderScoreUI.text += leader.score.ToString("N0") + "\n";

            counter++;
        }

        loadingPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

}
