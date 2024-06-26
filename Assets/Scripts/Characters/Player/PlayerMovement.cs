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
    public bool useNewTouchControls;

    public static PlayerMovement instance;

    private void Start() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        useNewTouchControls = PlayerPrefs.GetInt("UseNewTouchControls", 1) == 1;
        playerHealthManager.OnPlayerDied += DisableScript;
    }

    void Update() {
        Move();
        LimitPlayerMovement();
        EmitPlayerPositionY();
        DeactivateGround();
    }

    private Vector2 startTouchPosition, endTouchPosition;
    private Vector2 currentTouchPosition;

    private void Move() {

        // ================== Keyboard Controls ==================

        if (Input.GetButtonDown("Jump")) {
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            rb.velocity = new Vector2(-1 * moveXForce, 1 * 2);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            rb.velocity = new Vector2(1 * moveXForce, 1 * 2);
        }

        // ================== Touch Controls ==================

        ReadTouchControls();

    }


    private void ReadTouchControls() {
        // ================== New Touch Controls ==================
        if (useNewTouchControls) {
            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = touch.position;

                if (touchPosition.x < Screen.width / 3 && touch.phase == TouchPhase.Began) {
                    rb.velocity = new Vector2(-1 * moveXForce, 1 * 2);
                }
                else if (touchPosition.x > Screen.width / 3 + Screen.width / 3 && touch.phase == TouchPhase.Began) {
                    rb.velocity = new Vector2(1 * moveXForce, 1 * 2);
                }
                else if (touch.phase == TouchPhase.Began) {
                    rb.velocity = Vector2.up * jumpForce;
                }
            }
            return;
        }

        // ================== Old Touch Controls ==================

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            startTouchPosition = Input.GetTouch(0).position;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            endTouchPosition = Input.GetTouch(0).position;

            if (endTouchPosition.y >= startTouchPosition.y) {
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

    private void EmitPlayerPositionY() {
        OnPositionYChanged?.Invoke(transform.position.y);
    }

    private void DeactivateGround() {
        if (transform.position.y > 7.5f) {
            ground.SetActive(false);
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
        rb.velocity = Vector2.up * jumpForce;
    }
}

