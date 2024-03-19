using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInvincible : ItemBase {

    [Header("Item Invincible")]

    [SerializeField] private int durationInvencilityInSeconds = 5;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerHealthManager playerHealtManager = other.GetComponent<PlayerHealthManager>();
            playerHealtManager.SetInvencible(durationInvencilityInSeconds);
            Destroy(gameObject);
        }
    }
}
