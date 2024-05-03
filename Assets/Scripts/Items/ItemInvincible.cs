using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInvincible : AbstractItemBase {

    [Header("Item Invincible")]

    [SerializeField] private int effectDurationInSeconds = 5;

    private new void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.CompareTag("Player")) {
            PlayerHealthManager playerHealtManager = other.GetComponent<PlayerHealthManager>();
            playerHealtManager.SetInvencible(effectDurationInSeconds);
        }
    }
}
