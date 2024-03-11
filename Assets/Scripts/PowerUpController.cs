using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

    [SerializeField] private float speed = 1f;
    [SerializeField] private float distance = 2.5f;
    [SerializeField] private float distanceToDestroy = -10f;

    private Vector3 startPosition;
    private Transform playerTransform;

    private float time;


    void Start() {
        startPosition = transform.position;

        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update() {

        time += Time.deltaTime;

        float newPositionX = Mathf.Sin(Time.time * speed) * distance;
        float newPositionY = Time.time * speed;
        transform.position = startPosition + new Vector3(newPositionX, newPositionY, 0);

        if (transform.position.y < playerTransform.position.y + distanceToDestroy || time > 10f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }

}
