using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Dependencies")]
    [SerializeField] private GameObject ground;
    [SerializeField] private PlayerHealthManager playerHealthManager;
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float moveXForce = 4f;
    [SerializeField] private Rigidbody2D rb;

    [HideInInspector] public Action<float> OnPositionYChanged { get; set; }

    public static PlayerMovement instance;

    private void Start() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        playerHealthManager.OnPlayerDied += DisableScript;
    }

    void Update() {
        Move();
        LimitPlayerMovement();
        EmitPlayerPositionY();
        DestroyGround();
    }

    private Vector2 startTouchPosition, endTouchPosition;
    private Vector2 currentTouchPosition;

    private void Move() {

        // ================== Keyboard Controls ==================

        if (Input.GetButtonDown("Jump")) {
            rb.velocity = Vector2.up * jumpForce;
        }
        else if (Input.GetKeyDown(KeyCode.S) && isDownDashActive) {
            playerHealthManager.SetInvencible(0.4f);
            rb.velocity = Vector2.down * jumpForce;
            Invoke(nameof(StopDownForce), 0.3f);
        }

        //move to the left
        if (Input.GetKeyDown(KeyCode.A)) {
            rb.velocity = new Vector2(-1 * moveXForce, 1 * 2);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            rb.velocity = new Vector2(1 * moveXForce, 1 * 2);
        }


        // ================== Touch Controls ==================

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            endTouchPosition = Input.GetTouch(0).position;

            if (endTouchPosition.y < startTouchPosition.y && isDownDashActive) {
                rb.velocity = Vector2.down * jumpForce;
                Invoke(nameof(StopDownForce), 0.3f);
            }
            else if (endTouchPosition.y >= startTouchPosition.y) {
                rb.velocity = Vector2.up * jumpForce;
            }

            if (endTouchPosition.x + 100 < startTouchPosition.x) {
                // rb.velocity = Vector2.left * moveForce * Time.deltaTime;
                rb.velocity = new Vector2(-1 * moveXForce, 1 * 2);
            }
            else if (endTouchPosition.x - 100 > startTouchPosition.x) {
                // rb.velocity = Vector2.right * moveForce * Time.deltaTime;
                rb.velocity = new Vector2(1 * moveXForce, 1 * 2);

            }
        }

    }

    private void LimitPlayerMovement() {
        if (transform.position.x <= -1f) {
            transform.position = new Vector2(-1f, transform.position.y);
        }
        if (transform.position.x >= 1f) {
            transform.position = new Vector2(1f, transform.position.y);
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

    private void DestroyGround() {
        if (transform.position.y > 20) {
            Destroy(ground);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.TryGetComponent(out IDamageReceiver makeDamage) && other.transform.position.y < transform.position.y) {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void DisableScript() {
        this.enabled = false;
        Invoke(nameof(FreezePosition), 2f);
    }

    private void FreezePosition() {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void ResetValues() {
        this.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
    }
}

