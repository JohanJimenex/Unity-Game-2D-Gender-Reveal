using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealthManager playerHealthManager;
    // private readonly float smoothSpeed = 1f; // Añade una velocidad de suavizado a la camara

    public bool playerIsDead = false;

    public static CameraController instance;

    void Start() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        SubcribeAndListenToEvents();
        // lastPlayerPositionY = playerMovement.transform.position.y; // Añade un offse textra para que la cámara no siga al jugador al inicio del juego
    }

    private void SubcribeAndListenToEvents() {
        playerMovement.OnPositionYChanged += FollowThePlayerPosition; // Suscríbete al evento OnPositionYChanged del script del jugador
    }


    private float lastPlayerPositionY = 0;
    private readonly float distanceToDestroy = 5f;

    private void FollowThePlayerPosition(float playerPositionY) {
        if (playerPositionY > lastPlayerPositionY) {
            //     Vector3 newPosition = new Vector3(transform.position.x, playerPositionY, transform.position.z);
            //     Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPosition, smoothSpeed); // Usa Lerp para suavizar la transición
            //     transform.position = smoothedPosition;
            transform.position = new Vector3(transform.position.x, playerPositionY, transform.position.z);
            lastPlayerPositionY = playerPositionY;
        }

        CheckIfPlayerIsOutOfCam(playerPositionY);

    }

    private void CheckIfPlayerIsOutOfCam(float playerPositionY) {
        if (playerPositionY < transform.position.y - distanceToDestroy && !playerIsDead) {
            playerHealthManager.EnableCanReciveHurt(); // Llama al método IsDead del script del jugador
            playerHealthManager.ReceiveDamage(100);
            playerIsDead = true;
        }
    }
}