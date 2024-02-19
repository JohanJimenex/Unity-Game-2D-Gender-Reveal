using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {
    public float speed = 1f;
    public float distance = 2.5f;
    public float distanceToDestroy = -10f;
    private Vector3 startPosition;
    private Transform playerTransform;


    void Start() {
        startPosition = transform.position;

        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        float newPosition = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPosition + new Vector3(newPosition, 0, 0);

        if (transform.position.y < playerTransform.position.y + distanceToDestroy) {
            Destroy(gameObject);
        }
    }
}
