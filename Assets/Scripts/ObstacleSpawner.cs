using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
    public GameObject obstaclePrefab;
    public Transform playerTransform;

    public float spawnHeight = 10f;
    public float horizontalRangeToMove = 2.5f;
    public float distanceToSpawn = -2f;

    private float lastSpawnPositionY;

    void Update() {
        if (ShouldSpawn()) {
            SpawnObstacle();
        }
    }

    private bool ShouldSpawn() {
        return (playerTransform.position.y - lastSpawnPositionY) >= distanceToSpawn;
    }

    private void SpawnObstacle() {
        float randomPositionX = Random.Range(-horizontalRangeToMove, horizontalRangeToMove);
        Vector3 spawnPosition = new Vector3(randomPositionX, playerTransform.position.y + spawnHeight, 0);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;

        if (distanceToSpawn < 4f) {
            distanceToSpawn -= 0.1f;
        }
    }

}
