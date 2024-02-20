using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform playerTransform;
    public PlayerController playerScript; // Referencia al script del jugador
    public float smoothSpeed = 0.125f; // Añade una velocidad de suavizado

    private float lastPlayerPositionY;
    private readonly float distanceToDestroy = 5f;
    private bool playerIsDead = false;

    void Start() {
        lastPlayerPositionY = playerTransform.position.y + 3f; // Añade un offse textra para que la cámara no siga al jugador al inicio del juego
    }

    void Update() {
        if (playerTransform.position.y > lastPlayerPositionY) {
            Vector3 newPosition = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPosition, smoothSpeed); // Usa Lerp para suavizar la transición
            transform.position = smoothedPosition;
            lastPlayerPositionY = playerTransform.position.y;
        }
        if (playerTransform.position.y < transform.position.y - distanceToDestroy && !playerIsDead) {
            playerScript.IsDead(); // Llama al método IsDead del script del jugador
            playerIsDead = true;
        }
    }
}