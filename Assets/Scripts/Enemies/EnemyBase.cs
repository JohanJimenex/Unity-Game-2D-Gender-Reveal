using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IMakeDamage {

    [SerializeField] protected int enemyAttackForce = 1;
    [SerializeField] protected int enemyLifePoints = 1;

    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float distanceRangeToMoveX = 2.5f;
    [SerializeField] protected int scorePointsValue = 10;

    protected float distanceFromPlayerToDestroy = 10f;
    protected Vector3 startPosition;
    protected Transform playerTransform;

    void Start() {
        startPosition = transform.position;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void Update() {
        DestroyGameObjectIfFarFromPlayer();
    }

    private void DestroyGameObjectIfFarFromPlayer() {
        if (playerTransform.position.y > (startPosition.y + distanceFromPlayerToDestroy)) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.TryGetComponent<IMakeDamage>(out IMakeDamage makeDamage)) {

            makeDamage.MakeDamage(enemyAttackForce);

            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
        }
    }

    protected abstract void Move();

    public void MakeDamage(int damage) {
        enemyLifePoints -= damage;
        if (enemyLifePoints <= 0) {
            EnemyDead();
        }
    }

    private void EnemyDead() {
        GameManager.IncreaseScore(scorePointsValue);
        Destroy(gameObject, 1f);
    }
}
