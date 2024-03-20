using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {

    [SerializeField] protected int damage = 1;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float distanceRangeToMoveX = 2.5f;
    protected float distanceToDestroy = 10f;

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
        if (playerTransform.position.y > (startPosition.y + distanceToDestroy)) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {


        // MakeDamageToPlayer(other.gameObject.GetComponent<PlayerHealthManager>());
        if (other.gameObject.TryGetComponent<IMakeDamage>(out IMakeDamage makeDamage)) {

            makeDamage.MakeDamage(damage);

            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
            Destroy(gameObject, 1f);
        }
    }

    protected abstract void Move();

}
