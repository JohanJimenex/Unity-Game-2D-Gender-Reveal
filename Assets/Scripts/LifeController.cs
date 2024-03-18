using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {

    [SerializeField] private float verticalSpeedY = 2f;
    [SerializeField] private float horizontalSpeedX = 2f;
    [SerializeField] private float horizontalRangeDistanceToMove = 2f;

    private int horizontalDirection = 1;
    private float timeToDestroyGameObject;
    private readonly float distanceFromPlayerToDestroy = -10f;
    private Transform playerTransform;

    void Start() {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        Move();
        DestroyGameObjectIfOutOfRange();
    }

    private void Move() {

        transform.Translate(new Vector3(horizontalSpeedX * horizontalDirection, verticalSpeedY, 0) * Time.deltaTime);

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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }

}
