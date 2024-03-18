using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : EnemyBase {


    protected override void Update() {

        base.Update();

        Move();
    }

    protected override void Move() {
        float newPosition = Mathf.Sin(Time.time * base.moveSpeed) * base.distanceRangeToMoveX;
        transform.position = base.startPosition + new Vector3(newPosition, 0, 0);
    }

}
