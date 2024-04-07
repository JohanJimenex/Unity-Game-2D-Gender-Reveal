using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float moveForce = 10f;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject ground;

    [HideInInspector] public Action<float> OnPositionYChanged { get; set; }

    void Update() {
        Move();
        EmitPlayerPositionY();
        DeadIfPlayerFalls();
    }

    private Vector2 startTouchPosition, endTouchPosition;

    private void Move() {
        if (transform.position.x <= -1f) {
            transform.position = new Vector2(-1f, transform.position.y);
        }
        if (transform.position.x >= 1f) {
            transform.position = new Vector2(1f, transform.position.y);
        }

        // ================== Keyboard Controls ==================

        if (Input.GetButtonDown("Jump")) {
            rb.velocity = Vector2.up * moveForce;
        }

        if (Input.GetKeyDown(KeyCode.S) && isDownDashActive) {
            rb.velocity = Vector2.down * moveForce;
            Invoke(nameof(StopDownForce), 0.3f);
        }

        //move to the left
        if (Input.GetKeyDown(KeyCode.A)) {
            rb.velocity = Vector2.left * moveForce;
        }

        //move to the rigth
        if (Input.GetKeyDown(KeyCode.D)) {
            rb.velocity = Vector2.right * moveForce;
        }


        // ================== Touch Controls ==================

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
            endTouchPosition = Input.GetTouch(0).position;

            if (endTouchPosition.y < startTouchPosition.y && isDownDashActive) {
                rb.velocity = Vector2.down * moveForce;
                Invoke(nameof(StopDownForce), 0.3f);
            }
            else if (endTouchPosition.y >= startTouchPosition.y) {
                rb.velocity = Vector2.up * moveForce;
            }

            if (endTouchPosition.x + 100 < startTouchPosition.x) {
                // rb.velocity = Vector2.left * moveForce * Time.deltaTime;
                rb.velocity = Vector2.left * moveForce;
            }
            else if (endTouchPosition.x - 100 > startTouchPosition.x) {
                // rb.velocity = Vector2.right * moveForce * Time.deltaTime;
                rb.velocity = Vector2.right * moveForce;
            }

        }


        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                isDragging = true;
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging) {
                currentTouchPosition = touch.position;
                Vector2 direction = currentTouchPosition - startTouchPosition;
                // Mueve tu objeto en la dirección del arrastre aquí
            }
            else if (touch.phase == TouchPhase.Ended) {
                isDragging = false;
            }
        }

    }

    private bool isDragging = false;
    private Vector2 currentTouchPosition;


    private void StopDownForce() {
        rb.velocity = Vector2.up * 3;
    }

    private bool isDownDashActive = false;

    public void ActiveDownDash(int durationInSeconds) {
        isDownDashActive = true;
        Invoke(nameof(DeactivateDownDash), durationInSeconds);
    }

    private void DeactivateDownDash() {
        isDownDashActive = false;
    }

    private void EmitPlayerPositionY() {
        OnPositionYChanged?.Invoke(transform.position.y);
    }

    private void DeadIfPlayerFalls() {
        if (transform.position.y > 20) {
            Destroy(ground);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.TryGetComponent(out IDamageReceiver makeDamage) && other.transform.position.y < transform.position.y) {
            rb.velocity = Vector2.up * moveForce;
        }
    }
}

