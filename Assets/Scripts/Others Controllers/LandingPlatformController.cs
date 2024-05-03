using UnityEngine;

class LandingPlatformController : MonoBehaviour {

    [Header("Landing Platform Options")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int lifePointsToHealth = 2;
    [SerializeField] private int extraScorePointToGive = 50;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Animator anim;

    private PlayerHealthManager playerHealthManager;
    private Transform playerTransform;
    private float direction = 1.0f;

    private void Start() {
        playerHealthManager = GameObject.FindWithTag("Player").GetComponent<PlayerHealthManager>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update() {
        Move();
        ToggleBoxColliderBasedOnPlayerPosition();
        DestroyOnOutOfCamera();
    }

    private void Move() {
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        if (transform.position.x > 2.5f) {
            direction = -1.0f;
        }
        else if (transform.position.x < -2.5f) {
            direction = 1.0f;
        }
    }

    private void ToggleBoxColliderBasedOnPlayerPosition() {

        if (playerTransform.position.y + 0.4 >= transform.position.y) {
            boxCollider2D.enabled = true;
        }
        else {
            boxCollider2D.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            moveSpeed = 0;
            GameManager.IncreaseScore(extraScorePointToGive);
            playerHealthManager.IncreaseLife(lifePointsToHealth);
            anim.SetBool("PlayerIsOn", true);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            lifePointsToHealth = 0;
            extraScorePointToGive = 0;
            anim.SetBool("PlayerIsOn", true);
            anim.SetBool("PlayerIsOn", false);
        }
    }

    private void DestroyOnOutOfCamera() {
        if (playerTransform.position.y > transform.position.y + 6) {
            Destroy(gameObject);
        }
    }

}