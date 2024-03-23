using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperEnemyController : AbstractEnemyBase {

    [Header("Stopper Enemy Options")]

    [SerializeField] private float timeToMove = 2f;

    protected override void Update() {

        timeToMove -= Time.deltaTime;

        if (timeToMove <= 0) {
            Move();
            Invoke(nameof(ResetTimeToMove), 2f);
        }

        DestroyGameObjectIfFarFromPlayer();
    }

    private void ResetTimeToMove() {
        timeToMove = 2f;
    }

}
