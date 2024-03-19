using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private PlayerMovement playerMovement;


    public PlayerHealthManager playerHealthManager; // Referencia al script del jugador
    private readonly float smoothSpeed = 0.125f; // Añade una velocidad de suavizado a la camara

    private float lastPlayerPositionY;
    private readonly float distanceToDestroy = 5f;
    private bool playerIsDead = false;

    void Start() {

        playerMovement.OnPositionYChanged += UpdateCameraPosition; // Suscríbete al evento OnPositionYChanged del script del jugador

        lastPlayerPositionY = playerHealthManager.transform.position.y + 3f; // Añade un offse textra para que la cámara no siga al jugador al inicio del juego
    }

    private void UpdateCameraPosition(float playerPositionY) {

        if (playerPositionY > lastPlayerPositionY) {
            Vector3 newPosition = new Vector3(transform.position.x, playerPositionY, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPosition, smoothSpeed); // Usa Lerp para suavizar la transición
            transform.position = smoothedPosition;
            lastPlayerPositionY = playerPositionY;
        }
        if (playerPositionY < transform.position.y - distanceToDestroy && !playerIsDead) {
            playerHealthManager.MakeDamage(100); // Llama al método IsDead del script del jugador
            playerIsDead = true;
        }
    }
}