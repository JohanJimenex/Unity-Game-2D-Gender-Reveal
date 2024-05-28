using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IDamageReceiver, ILifeIncreaser {

    [SerializeField] private int lifes;
    [SerializeField] private bool canReciveDamage = true;

    [HideInInspector] public Action<int> OnPlayerGetDamage { get; set; }
    [HideInInspector] public Action<int> OnPlayerIncreaseLife { get; set; }
    [HideInInspector] public Action OnPlayerDied { get; set; }

    public int maxLifes;
    public static PlayerHealthManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        maxLifes = lifes;
    }

    public void SetInvencible(float duration) {
        canReciveDamage = false;
        Invoke(nameof(EnableCanReciveHurt), duration);
    }

    public void EnableCanReciveHurt() {
        canReciveDamage = true;
    }

    public void IncreaseLife(int quantity = 1) {
        if (lifes < maxLifes) {
            lifes += quantity;
            OnPlayerIncreaseLife?.Invoke(lifes);
        }
    }

    public void ReceiveDamage(int damage) {

        if (!canReciveDamage) return;

        lifes -= damage;

        OnPlayerGetDamage?.Invoke(damage);

        if (lifes <= 0) {
            PlayerDead();
        }
    }

    public void RevivePlayer() {
        lifes = maxLifes;
        OnPlayerIncreaseLife?.Invoke(lifes);
    }

    private void PlayerDead() {
        OnPlayerDied?.Invoke();
    }

}
