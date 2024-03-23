using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private PlayerMovement playerMovement;


    [SerializeField] private PlayerHealthManager playerHealthManager; // Referencia al script del jugador
    private readonly float smoothSpeed = 0.125f; // Añade una velocidad de suavizado a la camara

    private float lastPlayerPositionY;
    private readonly float distanceToDestroy = 5f;
    private bool playerIsDead = false;

    void Start() {
        SubcribeAndListenToEvents();
        lastPlayerPositionY = playerMovement.transform.position.y + 3f; // Añade un offse textra para que la cámara no siga al jugador al inicio del juego
    }

    private void SubcribeAndListenToEvents() {
        playerMovement.OnPositionYChanged += FollowThePlayerPosition; // Suscríbete al evento OnPositionYChanged del script del jugador
        playerHealthManager.OnPlayerGetDamage += ShakeCamera;
    }

    private void ShakeCamera(int _) {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake() {

        float elapsedTime = 0.0f;
        float duration = 0.1f;
        float magnitude = 0.01f;
        Vector3 originalPos = transform.position;

        while (elapsedTime < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }

    private void FollowThePlayerPosition(float playerPositionY) {
        if (playerPositionY > lastPlayerPositionY) {
            Vector3 newPosition = new Vector3(transform.position.x, playerPositionY, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPosition, smoothSpeed); // Usa Lerp para suavizar la transición
            transform.position = smoothedPosition;
            lastPlayerPositionY = playerPositionY;
        }

        if (playerPositionY < transform.position.y - distanceToDestroy && !playerIsDead) {
            playerHealthManager.ReceiveDamage(100); // Llama al método IsDead del script del jugador
            playerIsDead = true;
        }
    }
}