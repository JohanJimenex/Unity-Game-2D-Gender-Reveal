using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float upForce = 10f;
    public float downForce = -5f;
    public Animator animator;

    public GameObject ground;
    private Rigidbody2D rb;
    public Button buttonRestar;

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
        // GetComponent<SpriteRenderer>().enabled = false; //manejar en la animacion
        // Llama a la función "FunctionToCall" después de 5 segundos
        Invoke(nameof(ShowGameOverScreen), 1.2f);
    }

    private void ShowGameOverScreen() {
        buttonRestar.gameObject.SetActive(true);
    }
}
