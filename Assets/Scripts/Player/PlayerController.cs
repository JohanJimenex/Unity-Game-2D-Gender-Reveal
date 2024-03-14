using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField] private PlayerMovement playerMovement;

    private int lives = 2;
    public bool canReciveHurt = true;

    // [SerializeField] private float upForce = 10f;
    // [SerializeField] private float downForce = -5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    // [SerializeField] private GameObject ground;

    [HideInInspector] public Action<int> OnHurt { get; set; }
    // [HideInInspector] public Action<float> OnPositionYChanged { get; set; }
    [HideInInspector] public Action<int> OnPlayerGetLive { get; set; }
    [HideInInspector] public Action OnPlayerDied { get; set; }

    void Update() {

        // if (Input.touchCount > 0 || Input.GetButtonDown("Jump")) {
        //     rb.velocity = Vector2.up * upForce;
        // }

        // if (transform.position.y > 20) {
        //     Destroy(ground);
        // }

        // OnPositionYChanged?.Invoke(transform.position.y);
    }

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

        if (other.gameObject.CompareTag("Live")) {
            AddLive();
        }
    }

    private void EnableCanReciveHurt() {
        canReciveHurt = true;
    }

    private void Hurt() {

        lives--;
        OnHurt?.Invoke(lives);

        if (lives <= 0) {
            IsDead();
        }
    }

    private void AddLive() {
        if (lives < 2) {
            lives++;
            OnPlayerGetLive?.Invoke(lives);
        }
    }

    public void IsDead() {
        animator.SetTrigger("PlayerDying");
        rb.bodyType = RigidbodyType2D.Static;
        // GetComponent<SpriteRenderer>().enabled = false; //manejar en la animacion
        // Llama a la función "FunctionToCall" después de 5 segundos
        OnPlayerDied?.Invoke();
    }

}
