using UnityEngine;

class ItemMultiplyScore : AbstractItemBase {

    [Header("Item Multiply Score")]

    [SerializeField] private int multiplierScoreBy = 2;
    [SerializeField] private int effectDurationInSeconds = 5;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameManager.MultiplyScoreBy(multiplierScoreBy, effectDurationInSeconds);
            Destroy(gameObject);
        }
    }

}