using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExtraScore : AbstractItemBase {

    [Header("Item Extra Score")]

    private UIManager uIManager;

    private new void Start() {
        base.Start();
        uIManager = FindFirstObjectByType<UIManager>();
    }

    private new void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        int extraScore = GetRandomScorePoints();

        if (other.gameObject.CompareTag("Player")) {
            GameManager.IncreaseScore(extraScore);
            uIManager.ShowExtraScore(extraScore);
        }
    }

    private int GetRandomScorePoints() {
        return Random.Range(10, 100);
    }

}
