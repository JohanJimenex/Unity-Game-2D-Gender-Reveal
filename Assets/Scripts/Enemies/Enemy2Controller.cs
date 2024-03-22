using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : EnemyBase {

    private float direction = 1.0f;
    private float timeToMove = 2f;

    protected override void Update() {
        timeToMove -= Time.deltaTime;

        if (timeToMove <= 0) {
            Move();
            Invoke(nameof(ResetTimeToMove), 2f);
        }
    }

    private void ResetTimeToMove() {
        timeToMove = 2f;
    }

    protected override void Move() {

        float movement = direction * base.moveSpeed * Time.deltaTime;
        transform.Translate(movement, 0, 0);

        if (Mathf.Abs(transform.position.x - base.startPosition.x) >= base.distanceRangeToMoveX) {
            direction *= -1;
        }
    }
}
