using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IMakeDamage, IIncreaseLife {
    private int lifes = 2;
    public bool canReciveDamage = true;

    [SerializeField] private Rigidbody2D rb;

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

    public void MakeDamage(int damage) {

        if (!canReciveDamage) return;

        lifes -= damage;

        Handheld.Vibrate();

        OnPlayerGetDamage?.Invoke(lifes);

        if (lifes <= 0) {
            PlayerDead();
        }
    }

    private void PlayerDead() {
        rb.bodyType = RigidbodyType2D.Static;
        OnPlayerDied?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent<IMakeDamage>(out IMakeDamage makeDamage)) {
            OnPlayerGetDamage?.Invoke(lifes);
        }
    }

}
