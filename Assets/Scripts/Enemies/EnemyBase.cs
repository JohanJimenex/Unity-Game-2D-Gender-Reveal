using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestroyFrom {
    TOP = 1,
    BOTTOM = -1,
}

public abstract class EnemyBase : MonoBehaviour, IMakeDamage {

    [SerializeField] protected int enemyAttackForce = 1;
    [SerializeField] protected int enemyLifePoints = 1;

    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float distanceRangeToMoveX = 2.5f;
    [SerializeField] protected int scorePointsValue = 10;

    [Header("Destroy Options")]
    [SerializeField] protected bool canBeDestroyedWithoutDamage = false;
    [SerializeField] protected private DestroyFrom destroyFrom;

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

    private void DestroyGameObjectIfFarFromPlayer() {
        if (playerTransform.position.y > (startPosition.y + distanceFromPlayerToDestroy)) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        MakeDamageToOther(other);
    }

    private void MakeDamageToOther(Collision2D other) {
        if (other.gameObject.TryGetComponent<IMakeDamage>(out IMakeDamage makeDamage)) {

            if (canBeDestroyedWithoutDamage && other.transform.position.y <
                (transform.position.y * (float)DestroyFrom.TOP)) {
                return;
            }

            makeDamage.MakeDamage(enemyAttackForce);
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
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false; //desactivo el script para que no se mueva
        Destroy(gameObject, 1f);
    }
}
