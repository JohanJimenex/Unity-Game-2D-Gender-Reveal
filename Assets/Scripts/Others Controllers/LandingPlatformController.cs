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
        else if (transform.position.x < -2.5f) {
            direction = 1.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y) {
            moveSpeed = 0;
            GameManager.AddExtraScore(extraScorePointToGive);
            playerHealthManager.IncreaseLife(lifePointsToHealth);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y) {
            Destroy(gameObject, 1f);
        }
    }

}