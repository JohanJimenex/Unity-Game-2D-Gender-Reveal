using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
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
    [SerializeField] private GameObject newWorldRecordPanel;
    [SerializeField] private TMP_InputField nameInput;

    private int bestScore;
    private List<User> users;

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

    private void OnPlayerDied() {
        Invoke(nameof(ShowGameOverPanel), 1.2f);
        Invoke(nameof(ShowBestScoreText), 1.2f);
        Invoke(nameof(CheckWorldRecord), 1.2f);
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

    string userId;

    private async void CheckWorldRecord() {

        users = await firebaseConnection.ReadRecords();
        users.Reverse();

        if (users.Count < 30) {
            ShowNewWorldRecordPanel();
            return;
        }

        foreach (User user in users) {
            if (GameManager.GetScore() > user.data.score) {
                userId = user.firebaseId;
                ShowNewWorldRecordPanel();
                break;
            }
        }

    }

    private void ShowNewWorldRecordPanel() {
        AudioManager.instance.PlaySoundFx("Applause");
        newWorldRecordPanel.SetActive(true);
        nameInput.Select();
        nameInput.ActivateInputField();
    }

    public async void GetInputText() {
        string inputText = Utils.FilterBadWords(nameInput.text).Trim();
        int score = GameManager.GetScore();

        if (inputText.Length == 0) {
            inputText = "unknown";
        }

        firebaseConnection.WriteRecord(inputText, score);

        if (userId != null) {
            await firebaseConnection.DeleteRecord(userId);
            userId = null;
        }

        GetLeaderboard();
    }

    private void WriteNewLeader(String name, int score) {
        firebaseConnection.WriteRecord(name, score);
        GetLeaderboard();
    }

    public async void GetLeaderboard() {
        loadingPanel.SetActive(true);
        users = await firebaseConnection.ReadRecords();
        UpdateLeaderboardUI();
    }

    private void UpdateLeaderboardUI() {

        leadersNameUI.text = "";
        leaderScoreUI.text = "";
        int counter = 1;

        foreach (User leader in users) {

            String counterString = counter > 3 ? counter.ToString() : "  ";

            leadersNameUI.text += $"{counterString}  {leader.data.name.ToUpper()} \n";

            if (leader.data.score > 999999) {
                leader.data.score = 999999;
            }

            leaderScoreUI.text += leader.data.score.ToString("N0") + "\n";

            counter++;
        }

        loadingPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

}
