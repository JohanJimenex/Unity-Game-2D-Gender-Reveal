using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] PlayerMovement playerMovement;

    private static int score;
    private static int extraScore;
    private static int bestScore;

    private void Awake() {
        extraScore = 0;
        LoadBestScore();
    }

    private void Start() {
        playerMovement.OnPositionYChanged += IncreaseScore;
    }

    private void LoadBestScore() {
        bestScore = PlayerPrefs.GetInt("BestScore");
    }

    private void IncreaseScore(float skore) {

        score = extraScore + (int)skore;

        if (score > bestScore) {
            UpdateBestScore();
        }
    }

    public static void AddExtraScore(int extraSkore) {
        extraScore += extraSkore;
    }

    public static int GetScore() {
        return score;
    }

    private void UpdateBestScore() {
        PlayerPrefs.SetInt("BestScore", score);
    }

    public static void StartGame() {
        SceneManager.LoadScene("MainScene");
    }

}
