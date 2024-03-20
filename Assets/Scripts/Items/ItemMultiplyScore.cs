using UnityEngine;

class ItemMultiplyScore : ItemBase {

    [Header("Item Multiply Score")]

    [SerializeField] private int multiplierScoreBy = 2;
    [SerializeField] private int durationInSeconds = 5;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameManager.MultiplyScoreBy(2, durationInSeconds);
            Destroy(gameObject);
        }
    }

}