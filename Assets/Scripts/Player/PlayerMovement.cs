using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float upForce = 10f;
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

        // ================== Keyboard Controls ==================

        if (Input.GetButtonDown("Jump")) {
            rb.velocity = Vector2.up * upForce;
        }

        if (Input.GetKeyDown(KeyCode.S) && isDownDashActive) {
            rb.velocity = Vector2.down * upForce;
            Invoke(nameof(StopDownForce), 0.3f);
        }

        // ================== Touch Controls ==================

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            endTouchPosition = Input.GetTouch(0).position;

            if (endTouchPosition.y < startTouchPosition.y && isDownDashActive) {
                rb.velocity = Vector2.down * upForce;
                Invoke(nameof(StopDownForce), 0.3f);
            }
            else {
                rb.velocity = Vector2.up * upForce;
            }
        }
    }

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
            rb.velocity = Vector2.up * upForce;
        }
    }
}

