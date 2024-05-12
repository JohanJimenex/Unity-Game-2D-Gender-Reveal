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

    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float distanceRangeToMoveX = 2.5f;
    [SerializeField] protected int scorePointsValue = 10;

    [Header("Destroy Options")]
    // [SerializeField] protected bool canBeDestroyedWithoutDamage = false;
    [SerializeField] protected private CanBeDestroyFrom canBeDestroyFrom;

    private UIManager uiManager;

    protected Vector3 startPosition;
    protected Transform playerTransform;

    protected void Start() {
        startPosition = transform.position;
        playerTransform = GameObject.FindWithTag("Player").transform;
        uiManager = GameObject.FindFirstObjectByType<UIManager>();
    }

    protected virtual void Update() {
        Move();
        FlipSprite();
        DestroyGameObjectIfFarFromPlayer();
    }

    private float direction = 1.0f;

    protected virtual void Move() {

        float movement = direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement, 0, 0);

        if (Mathf.Abs(transform.position.x - startPosition.x) >= distanceRangeToMoveX) {
            direction *= -1;
        }
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
        GameManager.IncreaseScore(scorePointsValue);
        GetComponent<Collider2D>().enabled = false;
        uiManager.ShowExtraScore(scorePointsValue);
        // this.enabled = false; //desactivo el script para que no se mueva el enemigo
        moveSpeed = 0;
        GetComponent<Animator>().SetTrigger("Dead");
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
