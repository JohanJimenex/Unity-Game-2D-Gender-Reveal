using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExtraScore : AbstractItemBase {

    private new void Start() {
        base.Start();
    }

    private new void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        int extraScore = GetRandomScorePoints();

        if (other.gameObject.CompareTag("Player")) {
            GameManager.instance.IncreaseScore(extraScore);
            UIManager.instance.ShowExtraScore(extraScore);
        }
    }

    private int GetRandomScorePoints() {
        return Random.Range(10, 100);
    }

}
