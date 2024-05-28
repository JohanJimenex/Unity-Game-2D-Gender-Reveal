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
    [SerializeField] private GameObject alertImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject bestScoreUI;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI extraScoreText;
    [SerializeField] private TextMeshProUGUI x2Text;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private GameObject screenBorderDamage;

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject MusicPlayerUI;
    [SerializeField] private GameObject swipeDownIndicator;

    [Header("Leaderboard UI")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TextMeshProUGUI leadersNameUI;
    [SerializeField] private TextMeshProUGUI leaderScoreUI;
    [SerializeField] private GameObject newWorldRecordPanel;
    [SerializeField] private TMP_InputField inputField;

    private int bestScore;
    private List<User> users;

    public static UIManager instance;

    private void Awake() {
        //Singleton
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }

        LoadBestScore();
    }

    private void Start() {
        SubscribeAndListenEvents();
        oxygenSlider.maxValue = PlayerHealthManager.instance.maxLifes;
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

        int score = GameManager.instance.GetScore();
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

    public void ShowX2Text(float longTime) {
        x2Text.gameObject.SetActive(true);
        Invoke(nameof(HideX2Text), longTime);
    }

    private void HideX2Text() {
        x2Text.gameObject.SetActive(false);
    }

    string userId;

    private async void CheckWorldRecord() {

        users = await firebaseConnection.ReadRecords();
        users.Reverse();

        int score = GameManager.instance.GetScore();

        if (users.Count < 35 && score >= bestScore) {
            ShowNewWorldRecordPanel();
            return;
        }

        foreach (User user in users) {
            if (score > user.data.score && score >= bestScore) {
                userId = user.firebaseId;
                ShowNewWorldRecordPanel();
                break;
            }
        }

    }

    private void ShowNewWorldRecordPanel() {
        AudioManager.instance.PlaySoundFx("Applause");
        newWorldRecordPanel.SetActive(true);
        inputField.Select();
        inputField.ActivateInputField();
    }

    public async void GetInputText() {
        string inputText = Utils.BadWordsFilter(inputField.text).Trim();
        int score = GameManager.instance.GetScore();

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

            string counterString = counter > 3 ? counter.ToString() : "  ";

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

    public void ShowAlertImage(Vector3 position) {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        alertImage.transform.position = new Vector3(screenPosition.x, alertImage.transform.position.y, 0);
        alertImage.GetComponent<Animator>().SetTrigger("Alert On");
        AudioManager.instance.PlaySoundFx("Alert");
    }

    public void ShowMusicPlayerUI(string musicName) {
        MusicPlayerUI.GetComponentInChildren<TextMeshProUGUI>().text = musicName;
        MusicPlayerUI.SetActive(true);
        Invoke(nameof(HideMusicPlayerUI), 4f);
    }

    private void HideMusicPlayerUI() {
        MusicPlayerUI.SetActive(false);
    }

    private bool firstTimeUsingSwipeDown = true;

    public void ShowSwipeDownIndicator() {

        if (!firstTimeUsingSwipeDown) {
            return;
        }
        swipeDownIndicator.SetActive(true);
        Invoke(nameof(HideSwipeDownIndicator), 2.8f);
        firstTimeUsingSwipeDown = false;
    }

    private void HideSwipeDownIndicator() {
        swipeDownIndicator.SetActive(false);
    }

}
