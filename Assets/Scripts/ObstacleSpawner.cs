using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour {
    public GameObject obstaclePrefab;
    public Transform playerTransform;

    public float spawnHeight = 10f;
    public float spawnRange = 2.5f;
    public float distanceThreshold = -2f;

    private float lastSpawnPositionY;

    void Update() {
        if (ShouldSpawn()) {
            SpawnObstacle();
        }
    }

    private bool ShouldSpawn() {
        return (playerTransform.position.y - lastSpawnPositionY) >= distanceThreshold;
    }

    private void SpawnObstacle() {
        float randomPositionX = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPosition = new Vector3(randomPositionX, playerTransform.position.y + spawnHeight, 0);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;
    }
}
