using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExtraScore : AbstractItemBase {

    [Header("Item Extra Score")]
    [SerializeField] private int extraScore = 20;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            GameManager.IncreaseScore(extraScore);
            Destroy(gameObject);
        }
    }

}
