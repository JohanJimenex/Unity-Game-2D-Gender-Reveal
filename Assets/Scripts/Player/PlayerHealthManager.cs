using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageReceiver, ILifeIncreaser {

    [SerializeField] private int lifes;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool canReciveDamage = true;

    [HideInInspector] public Action<int> OnPlayerGetDamage { get; set; }
    [HideInInspector] public Action<int> OnPlayerIncreaseLife { get; set; }
    [HideInInspector] public Action OnPlayerDied { get; set; }

    public void SetInvencible(int duration) {
        canReciveDamage = false;
        Invoke(nameof(EnableCanReciveHurt), duration);
    }

    private void EnableCanReciveHurt() {
        canReciveDamage = true;
    }

    public void IncreaseLife(int quantity = 1) {
        if (lifes < 2) {
            lifes += quantity;
        }
        OnPlayerIncreaseLife?.Invoke(lifes);
    }

    public void ReceiveDamage(int damage) {

        if (!canReciveDamage) return;

        lifes -= damage;

        Handheld.Vibrate();

        OnPlayerGetDamage?.Invoke(damage);

        if (lifes <= 0) {
            PlayerDead();
        }
    }

    private void PlayerDead() {
        rb.bodyType = RigidbodyType2D.Static;
        OnPlayerDied?.Invoke();
    }

}
