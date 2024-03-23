using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] private int playerAttackForce = 1;

    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.TryGetComponent<IDamageReceiver>(out IDamageReceiver makeDamage)) {
            makeDamage.ReceiveDamage(playerAttackForce);
        }
    }

}