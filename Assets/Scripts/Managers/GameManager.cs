using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] PlayerMovement playerMovement;

    private int score;
    private int bestScore;
    private int multiplierScoreBy = 1;
    //Singleton, para cuando se reinicia la escena
    public static GameManager instance;

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

    public void IncreaseScore(int skore) {

        skore *= multiplierScoreBy;

        score += skore;

        if (score > bestScore) {
            UpdateBestScore();
        }
    }


    public int GetScore() {
        return score;
    }

    public void MultiplyScoreBy(int multiplier, int durationInSeconds) {
        multiplierScoreBy = multiplier;
        instance.Invoke(nameof(ResetMultiplierScore), durationInSeconds);

    }

    private void ResetMultiplierScore() {
        multiplierScoreBy = 1;
    }

    private void UpdateBestScore() {
        PlayerPrefs.SetInt("BestScore", score);
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

}
