using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesSpawner : MonoBehaviour {


    [SerializeField] private GameObject[] enemiesPrefabs;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private float horizontalRangeToInstanciate = 2.5f;
    [SerializeField] private float distanceFromPlayerToSpawnY = -2f;

    private float lastSpawnPositionY;

    void Update() {

        if (CanSpawn()) {
            SpawnEnemy(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)]);
        }
    }

    private bool CanSpawn() {
        return (playerTransform.position.y - lastSpawnPositionY) >= distanceFromPlayerToSpawnY;
    }

    private void SpawnEnemy(GameObject enemyPrefab) {

        float randomPositionX = Random.Range(-horizontalRangeToInstanciate, horizontalRangeToInstanciate);
        Vector3 spawnPosition = new Vector3(randomPositionX, playerTransform.position.y + spawnHeight, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;

        if (distanceFromPlayerToSpawnY < 4f) {
            distanceFromPlayerToSpawnY -= 0.1f;
        }
    }
}
