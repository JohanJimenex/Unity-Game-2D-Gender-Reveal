using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CanBeDestroyFrom {
    NoWhere = 0,
    Top = 1,
    Bottom = -1,
}

public abstract class AbstractEnemyBase : MonoBehaviour, IDamageReceiver {

    [SerializeField] protected int enemyAttackForce = 1;
    [SerializeField] protected int enemyLifePoints = 1;

    [SerializeField] protected float moveXSpeed = 1f;
    [SerializeField] protected float moveYSpeed = 0f; 
    [SerializeField] protected float distanceRangeToMoveX = 2.5f;
    [SerializeField] protected int scorePointsValue = 10;

    [Header("Destroy Options")]
    // [SerializeField] protected bool canBeDestroyedWithoutDamage = false;
    [SerializeField] protected private CanBeDestroyFrom canBeDestroyFrom;

    protected Vector3 startPosition;
    protected Transform playerTransform;
    private float direction;

    protected void Start() {
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
        startPosition = transform.position;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void Update() {
        Move();
        FlipSprite();
        DestroyGameObjectIfFarFromPlayer();
    }

    protected virtual void Move() {

        // float movementX = direction * moveXSpeed * Time.deltaTime;
        // float movementY = direction * moveYSpeed * Time.deltaTime;

        Vector3 movement = new Vector3(direction * moveXSpeed, moveYSpeed, 0) * Time.deltaTime;

        transform.Translate(movement);

        if (Mathf.Abs(transform.position.x) > distanceRangeToMoveX) {
            direction = direction == 1f ? -1f : 1f;
        }

        // if (Mathf.Abs(transform.position.x - startPosition.x) >= distanceRangeToMoveX) {
        //     direction *= -1;
        // }
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if ((canBeDestroyFrom == CanBeDestroyFrom.Top && other.GetContact(0).normal.y < 0) ||
            (canBeDestroyFrom == CanBeDestroyFrom.Bottom && other.GetContact(0).normal.y > 0)) {
            return;
        }

        MakeDamageToOther(other);
    }

    private void MakeDamageToOther(Collision2D other) {
        if (other.gameObject.TryGetComponent<IDamageReceiver>(out IDamageReceiver makeDamage)) {
            makeDamage.ReceiveDamage(enemyAttackForce);
        }
    }

    public void ReceiveDamage(int damage) {
        enemyLifePoints -= damage;
        if (enemyLifePoints <= 0) {
            EnemyDead();
        }
    }

    private void EnemyDead() {
        GameManager.instance.IncreaseScore(scorePointsValue);
        GetComponent<Collider2D>().enabled = false;
        UIManager.instance.ShowExtraScore(scorePointsValue);
        moveXSpeed = 0;
        GetComponent<Animator>().SetTrigger("Dead");
        AudioManager.instance.PlaySoundFx("Explosion");
        Destroy(gameObject, 1f);
    }

    protected void FlipSprite() {
        if (direction > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected void DestroyGameObjectIfFarFromPlayer() {

        float distanceFromPlayerToDestroy = 10f;

        if (playerTransform.position.y > (transform.position.y + distanceFromPlayerToDestroy)) {
            Destroy(gameObject);
        }
    }

}
