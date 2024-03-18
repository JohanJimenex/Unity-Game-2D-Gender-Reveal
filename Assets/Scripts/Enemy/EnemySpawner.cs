using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour {

    public Transform playerTransform;

    [SerializeField] private GameObject[] enemyListPrefab;

    public float spawnHeight = 10f;
    public float horizontalRangeToInstanciate = 2.5f;
    public float distanceFromPlayerToSpawnY = -2f;

    private float lastSpawnPositionY;

    void Update() {

        if (ShouldSpawn()) {
            SpawnObstacle(enemyListPrefab[Random.Range(0, enemyListPrefab.Length)]);
        }
    }

    private bool ShouldSpawn() {
        return (playerTransform.position.y - lastSpawnPositionY) >= distanceFromPlayerToSpawnY;
    }

    private void SpawnObstacle(GameObject enemyPrefab) {

        float randomPositionX = Random.Range(-horizontalRangeToInstanciate, horizontalRangeToInstanciate);
        Vector3 spawnPosition = new Vector3(randomPositionX, playerTransform.position.y + spawnHeight, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;

        if (distanceFromPlayerToSpawnY < 4f) {
            distanceFromPlayerToSpawnY -= 0.1f;
        }
    }
}
