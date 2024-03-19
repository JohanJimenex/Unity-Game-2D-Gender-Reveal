using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLife : ItemBase {

    [Header("Item Life")]
    [SerializeField] private int lifePointsToIncreases = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerHealthManager playerHealtManager = other.GetComponent<PlayerHealthManager>();
            playerHealtManager.IncreasePlayerLife(lifePointsToIncreases);
            Destroy(gameObject);
        }
    }
}
