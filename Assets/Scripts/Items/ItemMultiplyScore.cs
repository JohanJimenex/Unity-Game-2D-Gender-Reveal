using UnityEngine;

class ItemMultiplyScore : AbstractItemBase {

    [Header("Item Multiply Score")]

    [SerializeField] private int multiplierScoreBy = 2;
    [SerializeField] private int effectDurationInSeconds = 5;

    private new void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Player")) {
            GameManager.instance.MultiplyScoreBy(multiplierScoreBy, effectDurationInSeconds);
            UIManager.instance.ShowX2Text(effectDurationInSeconds);
        }
    }

}