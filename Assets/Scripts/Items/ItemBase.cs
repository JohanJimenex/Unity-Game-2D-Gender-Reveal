using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour {

    [Header("Item Base")]

    [SerializeField] private float verticalSpeedY = 2f;
    [SerializeField] private float horizontalSpeedX = 2f;
    [SerializeField] private float horizontalRangeDistanceToMove = 2f;

    private int verticalDirection;
    private int horizontalDirection = 1;
    private float timeToDestroyGameObject;
    private readonly float distanceFromPlayerToDestroy = -10f;
    private Transform playerTransform;

    void Start() {
        verticalDirection = Random.Range(-1, 1) == 0 ? -1 : 1;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        Move();
        DestroyGameObjectIfOutOfRange();
    }

    private void Move() {

        transform.Translate(new Vector3(horizontalSpeedX * horizontalDirection, verticalDirection * verticalSpeedY, 0) * Time.deltaTime);

        // Invertir la dirección horizontal cuando llegue a los límites
        if (transform.position.x >= horizontalRangeDistanceToMove)
            horizontalDirection = -1;
        else if (transform.position.x <= -horizontalRangeDistanceToMove)
            horizontalDirection = 1;
    }

    private void DestroyGameObjectIfOutOfRange() {
        timeToDestroyGameObject += Time.deltaTime;

        if (transform.position.y <= (playerTransform.position.y + distanceFromPlayerToDestroy) || timeToDestroyGameObject >= 10f) {
            Destroy(gameObject);
        }
    }

}
