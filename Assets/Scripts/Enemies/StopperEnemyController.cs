using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperEnemyController : AbstractEnemyBase {

    [Header("Stopper Enemy Options")]

    [SerializeField] private float timeToWaitForMove = 2f;

    protected override void Update() {

        timeToWaitForMove -= Time.deltaTime;

        if (timeToWaitForMove >= 0) {
            Move();
        }

        if (timeToWaitForMove < -2) {
            timeToWaitForMove = 2f;
        }

        DestroyGameObjectIfFarFromPlayer();
    }



}
