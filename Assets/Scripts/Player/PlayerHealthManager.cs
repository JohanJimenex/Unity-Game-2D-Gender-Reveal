using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {
    private int lifes = 2;
    public bool canReciveHurt = true;

    [SerializeField] private Rigidbody2D rb;

    [HideInInspector] public Action<int> OnHurt { get; set; }
    [HideInInspector] public Action<int> OnPlayerGetLive { get; set; }
    [HideInInspector] public Action OnPlayerDied { get; set; }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy") && canReciveHurt) {
            Hurt();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("PowerUp")) {
            canReciveHurt = false;
            Invoke(nameof(EnableCanReciveHurt), 5f);
        }

        if (other.gameObject.CompareTag("Life")) {
            AddLife();
        }
    }

    private void EnableCanReciveHurt() {
        canReciveHurt = true;
    }

    private void Hurt() {

        lifes--;
        OnHurt?.Invoke(lifes);

        if (lifes <= 0) {
            IsDead();
        }
    }

    private void AddLife() {
        if (lifes < 2) {
            lifes++;
            OnPlayerGetLive?.Invoke(lifes);
        }
    }

    public void IsDead() {
        rb.bodyType = RigidbodyType2D.Static;
        OnPlayerDied?.Invoke();
    }
}
