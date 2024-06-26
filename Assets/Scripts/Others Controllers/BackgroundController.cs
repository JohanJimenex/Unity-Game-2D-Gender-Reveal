using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    [Header("Dependecies")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Background Speed")]
    [SerializeField] private float verticalSpeed = -0.10f;
    [SerializeField] private float horizontalSpeed = -0.10f;
    [SerializeField] private float extraVerticalSpeed = 0.10f;

    [Header("Screen Limits")]
    [SerializeField] private float maxLimitY = 16;
    [SerializeField] private float maxLimitX = 7;

    [Header("Re-positions points")]
    [SerializeField] private float repositionY = 16;
    [SerializeField] private float repositionX = 7;

    private float initialVerticalSpeed;

    private void Start() {
        playerMovement.OnPositionYChanged += AddExtraVerticalSpeed;
        initialVerticalSpeed = verticalSpeed;
    }

    private void LateUpdate() {
        TranslateBackgroundPosition();
        HandleScreenLimits();
    }

    private void TranslateBackgroundPosition() {
        transform.Translate(new Vector3(horizontalSpeed, verticalSpeed, 0) * Time.deltaTime);
    }

    private void HandleScreenLimits() {

        if (transform.position.x < -maxLimitX) {
            transform.position = new Vector3(repositionX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > maxLimitX) {
            transform.position = new Vector3(-repositionX, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -maxLimitY) {
            transform.position = new Vector3(transform.position.x, repositionY, transform.position.z);
        }
        // if (transform.position.y > maxLimitY) {
        //     transform.position = new Vector3(transform.position.x, -repositionY, transform.position.z);
        // }

    }

    private float tempPositionY = 0;

    private void AddExtraVerticalSpeed(float positionY = 0) {

        if (extraVerticalSpeed == 0) {
            return;
        }

        if (positionY >= tempPositionY) {
            tempPositionY = positionY;
            verticalSpeed = extraVerticalSpeed;
        }
        else {
            verticalSpeed = initialVerticalSpeed;
        }

    }


}
