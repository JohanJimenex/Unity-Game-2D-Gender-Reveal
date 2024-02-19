using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float upForce = 10f;
    public float downForce = -5f;
    public Animator animator;

    public GameObject ground;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {

        if (Input.touchCount > 0 || Input.GetButtonDown("Jump")) {
            rb.velocity = Vector2.up * upForce;
        }

        if (transform.position.y > 20) {
            Destroy(ground);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Obstacle")) {
            IsDead();
        }
    }

    public void IsDead() {
        animator.SetTrigger("Die");
        rb.bodyType = RigidbodyType2D.Static;
        print("Game Over");
    }
}
