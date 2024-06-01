using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDownDash : AbstractItemBase {

    [Header("Item Down Dash")]

    [SerializeField] private int effectDurationInSeconds = 15;

    private new void OnTriggerEnter2D(Collider2D other) {

        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Player")) {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            // playerMovement.ActiveDownDash(effectDurationInSeconds);

            PlayerAnim playerAnim = other.GetComponent<PlayerAnim>();
            playerAnim.ActiveDownDash(effectDurationInSeconds);
            UIManager.instance.ShowSwipeDownIndicator();
        }
    }

}
