using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInvencible : ItemBase {

    [Header("Item Invencible")]

    [SerializeField] private int durationInvencilityInSeconds = 5;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerHealthManager playerHealtManager = other.GetComponent<PlayerHealthManager>();
            playerHealtManager.SetInvencible(durationInvencilityInSeconds);
            Destroy(gameObject);
        }
    }
}
