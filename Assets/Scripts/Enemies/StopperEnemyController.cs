using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperEnemyController : AbstractEnemyBase {

    [Header("Stopper Enemy Options")]
    [Tooltip("Time to wait before moving again in seconds.")]
    [SerializeField] private float timeToWaitForMove = 2f;

    protected override void Update() {

        timeToWaitForMove -= Time.deltaTime;

        if (timeToWaitForMove >= 0) {
            base.Move();
        }

        if (timeToWaitForMove < -2) {
            timeToWaitForMove = 2f;
        }

        base.FlipSprite();
        base.DestroyGameObjectIfFarFromPlayer();
    }

}
