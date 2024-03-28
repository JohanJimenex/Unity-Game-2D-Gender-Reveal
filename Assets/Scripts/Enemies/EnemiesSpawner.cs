using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesSpawner : MonoBehaviour {


    [SerializeField] private GameObject[] easyEnemiesPrefabs;
    [SerializeField] private GameObject[] hardEnemiesPrefabs;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private float horizontalRangeToInstanciate = 2.5f;
    [SerializeField] private float distanceFromPlayerToSpawnY = -2f;

    private float lastSpawnPositionY;

    private float difficultyIncreaseHeight = 100f;
    private float hardesEnemiesPropability = 0;

    void Update() {

        if (CanSpawn()) {

            if (Random.Range(0, 100) < hardesEnemiesPropability) {
                SpawnEnemy(hardEnemiesPrefabs[Random.Range(0, hardEnemiesPrefabs.Length)]);
            }
            else {
                SpawnEnemy(easyEnemiesPrefabs[Random.Range(0, easyEnemiesPrefabs.Length)]);
            }
        }

        UpdateDifficultyPropability();
    }

    private void UpdateDifficultyPropability() {

        if (playerTransform.position.y >= difficultyIncreaseHeight) {
            difficultyIncreaseHeight += 50f;
            hardesEnemiesPropability += 5f;
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
