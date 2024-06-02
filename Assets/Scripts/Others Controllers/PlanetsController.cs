using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsController : MonoBehaviour {
    [SerializeField] private float verticalSpeed = 0f;
    [SerializeField] private float horizontalSpeed = 0f;

    private readonly float limit = -5f;

    private void Start() {
        //Random orientation
        horizontalSpeed = horizontalSpeed * Random.Range(0, 2) == 0 ? -1 : 1;
    }

    void Update() {
        Move();
        DestroyIfOutOfLimits();
    }

    private void Move() {
        transform.Translate(new Vector3(horizontalSpeed, verticalSpeed, 0) * Time.deltaTime);
    }


    private void DestroyIfOutOfLimits() {
        // comprobar si el objeto ha alcanzado el límite
        if (transform.position.y <= limit) {
            // destruir el objeto
            Destroy(gameObject);
        }
    }
}
