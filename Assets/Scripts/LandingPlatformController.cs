using UnityEngine;

class LandingPlatformController : MonoBehaviour {

    [Header("Landing Platform Options")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int lifePointsToHealth = 2;
    [SerializeField] private int extraScorePointToGive = 50;

    private PlayerHealthManager playerHealthManager;

    private float direction = 1.0f;

    private void Start() {
        playerHealthManager = GameObject.FindWithTag("Player").GetComponent<PlayerHealthManager>();
    }

    private void Update() {
        Move();
    }

    private void Move() {
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        if (transform.position.x > 2.5f) {
            direction = -1.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            moveSpeed = 0;
            GameManager.AddExtraScore(extraScorePointToGive);
            playerHealthManager.IncreaseLife(lifePointsToHealth);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Destroy(gameObject, 1f);
        }
    }

}