using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] PlayerMovement playerMovement;

    private static int score;
    private static int bestScore;
    private static int multiplierScoreBy = 1;
    //Singleton, para cuando se reinicia la escena
    private static GameManager instance;

    private void Awake() {

        score = 0;

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        LoadBestScore();
    }

    private void LoadBestScore() {
        bestScore = PlayerPrefs.GetInt("BestScore");
    }

    private void Start() {
        SubscribeAndListenToEvent();
    }

    private float actualPosition = 0;

    private void SubscribeAndListenToEvent() {

        playerMovement.OnPositionYChanged += (newPositionY) => {
            if (newPositionY > actualPosition) {
                actualPosition += 1;
                IncreaseScore(1);
            };
        };
    }

    public static void IncreaseScore(int skore) {

        skore *= multiplierScoreBy;

        score += skore;

        if (score > bestScore) {
            UpdateBestScore();
        }
    }


    public static int GetScore() {
        return score;
    }

    public static void MultiplyScoreBy(int multiplier, int durationInSeconds) {
        multiplierScoreBy = multiplier;
        instance.Invoke(nameof(ResetMultiplierScore), durationInSeconds);

    }

    private void ResetMultiplierScore() {
        multiplierScoreBy = 1;
    }

    private static void UpdateBestScore() {
        PlayerPrefs.SetInt("BestScore", score);
    }

    public static void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

}
