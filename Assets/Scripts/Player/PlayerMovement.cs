using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float upForce = 10f;
    // [SerializeField] private float downForce = -5f;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject ground;

    [HideInInspector] public Action<float> OnPositionYChanged { get; set; }

    void Update() {

        if (Input.touchCount > 0 || Input.GetButtonDown("Jump")) {
            rb.velocity = Vector2.up * upForce;
        }

        if (transform.position.y > 20) {
            Destroy(ground);
        }

        OnPositionYChanged?.Invoke(transform.position.y);
    }


}
