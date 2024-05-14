using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PugController : MonoBehaviour {

    [SerializeField] RectTransform rectTransform;
    [SerializeField] private float distance = 500.0f;
    [SerializeField] private float speed = 300f;

    private int direction = 1;

    void Update() {
        Move();
    }

    private void Move() {

        Vector3 position = rectTransform.anchoredPosition;

        position.x += speed * direction * Time.deltaTime;

        rectTransform.anchoredPosition = position;

        if (position.x > distance) {
            direction = -1;
            rectTransform.localScale = new Vector3(-1, 1, 1); //
        }
        else if (position.x < -distance) {
            direction = 1;
            rectTransform.localScale = new Vector3(1, 1, 1); //
        }

    }
}
