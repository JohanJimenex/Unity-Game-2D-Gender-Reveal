using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLife : AbstractItemBase {

    [Header("Item Life")]
    [SerializeField] private int lifePointsToIncreases = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.TryGetComponent<ILifeIncreaser>(out ILifeIncreaser increaseLife)) {
            increaseLife.IncreaseLife(lifePointsToIncreases);
            Destroy(gameObject);
        }
    }
}
