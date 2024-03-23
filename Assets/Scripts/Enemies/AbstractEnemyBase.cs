using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CanBeDestroyFrom {
    TOP = 1,
    BOTTOM = -1,
}

public abstract class AbstractEnemyBase : MonoBehaviour, IDamageReceiver {

    [SerializeField] protected int enemyAttackForce = 1;
    [SerializeField] protected int enemyLifePoints = 1;

    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float distanceRangeToMoveX = 2.5f;
    [SerializeField] protected int scorePointsValue = 10;

    [Header("Destroy Options")]
    [SerializeField] protected bool canBeDestroyedWithoutDamage = false;
    [SerializeField] protected private CanBeDestroyFrom canBeDestroyFrom;

    protected float distanceFromPlayerToDestroy = 10f;
    protected Vector3 startPosition;
    protected Transform playerTransform;

    void Start() {
        startPosition = transform.position;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void Update() {
        Move();
        DestroyGameObjectIfFarFromPlayer();
    }

    protected void DestroyGameObjectIfFarFromPlayer() {
        if (playerTransform.position.y > (startPosition.y + distanceFromPlayerToDestroy)) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        MakeDamageToOther(other);
    }

    private void MakeDamageToOther(Collision2D other) {
        if (other.gameObject.TryGetComponent<IDamageReceiver>(out IDamageReceiver makeDamage)) {

            if (canBeDestroyedWithoutDamage && other.transform.position.y <
                (transform.position.y * (float)CanBeDestroyFrom.TOP)) {
                return;
            }

            makeDamage.ReceiveDamage(enemyAttackForce);
        }
    }
    
    private float direction = 1.0f;

    protected virtual void Move() {

        float movement = direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement, 0, 0);

        if (Mathf.Abs(transform.position.x - startPosition.x) >= distanceRangeToMoveX) {
            direction *= -1;
        }
    }

    public void ReceiveDamage(int damage) {
        enemyLifePoints -= damage;
        if (enemyLifePoints <= 0) {
            EnemyDead();
        }
    }

    private void EnemyDead() {
        GameManager.AddExtraScore(scorePointsValue);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false; //desactivo el script para que no se mueva el enemigo
        Destroy(gameObject, 1f);
    }
}
