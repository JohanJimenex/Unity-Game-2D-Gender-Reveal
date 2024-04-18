using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float verticalSpeed = -0.10f;
    [SerializeField] private float horizontalSpeed = -0.10f;

    [SerializeField] private float extraVerticalSpeed = 0.10f;

    [SerializeField] private PlayerMovement playerMovement;

    private float initialVerticalSpeed;

    private void Start() {
        playerMovement.OnPositionYChanged += AddExtraVerticalSpeed;
        initialVerticalSpeed = verticalSpeed;
    }

    private float tempPositionY = 0;

    private void AddExtraVerticalSpeed(float positionY = 0) {

        if (positionY >= tempPositionY) {
            verticalSpeed = extraVerticalSpeed;
            tempPositionY = positionY;
        }
        else {
            verticalSpeed = initialVerticalSpeed;
        }

    }

    private void LateUpdate() {
        TranslateBackgroundPosition();
    }

    private void TranslateBackgroundPosition() {

        transform.Translate(new Vector3(horizontalSpeed, verticalSpeed, transform.position.z) * Time.deltaTime);

        if (transform.position.y <= -16) {
            RepositionYBackground();
        }

        if (transform.position.x <= -7) {
            RepositionXBackground();
        }
    }

    private void RepositionYBackground() {
        transform.position = new Vector3(cameraTransform.position.x, 16, transform.position.z);
    }

    private void RepositionXBackground() {
        transform.position = new Vector3(7, transform.position.y, transform.position.z);
    }

}
