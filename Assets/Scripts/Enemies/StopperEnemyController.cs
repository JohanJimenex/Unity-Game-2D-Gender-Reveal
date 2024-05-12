using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopperEnemyController : AbstractEnemyBase {

    [Header("Stopper Enemy Options")]
    [Tooltip("Time to wait before moving again in seconds.")]
    [SerializeField] private float timeToWaitForMove;

    private float counter;

    private new void Start() {
        base.Start();
        counter = timeToWaitForMove;
    }

    protected override void Update() {

        counter -= Time.deltaTime;

        if (counter >= 0) {
            base.Move();
        }

        if (counter < -timeToWaitForMove) {
            counter = timeToWaitForMove;
        }

        base.FlipSprite();
        base.DestroyGameObjectIfFarFromPlayer();
    }

}
