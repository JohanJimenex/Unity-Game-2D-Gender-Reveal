using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesSpawner : MonoBehaviour {

    [Header("Dependencies")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject[] easyEnemiesPrefabs;
    [SerializeField] private GameObject[] hardEnemiesPrefabs;
    [SerializeField] private GameObject[] descentEnemiesPrefabs;

    [Header("Settings")]
    [SerializeField] private float horizontalRangeToInstanciate = 2.5f;
    [SerializeField] private float distanceFromPlayerToSpawnY = -2f;

    private float positionToSpawnInY = 10f;

    private float heightGoalToIncreaseDifficulty = 100f;
    private float hardesEnemiesPropability = 0;
    private float descentEnemiesPropability = 10;

    void Update() {

        if (CanSpawn()) {

            if (Random.Range(0, 100) < hardesEnemiesPropability) {
                SpawnEnemy(hardEnemiesPrefabs[Random.Range(0, hardEnemiesPrefabs.Length)]);
            }
            else {
                SpawnEnemy(easyEnemiesPrefabs[Random.Range(0, easyEnemiesPrefabs.Length)]);
            }

            if (Random.Range(0, 100) < descentEnemiesPropability && Random.Range(0, 100) < 50) {
                ShowAlert(descentEnemiesPrefabs[Random.Range(0, descentEnemiesPrefabs.Length)]);
            }
        }

        UpdateDifficultyPropability();
    }
    private float lastSpawnPositionY;

    private bool CanSpawn() {
        return (playerTransform.position.y - lastSpawnPositionY) >= distanceFromPlayerToSpawnY;
    }

    private void UpdateDifficultyPropability() {

        if (playerTransform.position.y >= heightGoalToIncreaseDifficulty) {
            heightGoalToIncreaseDifficulty += 50f;
            hardesEnemiesPropability += 15f;
            descentEnemiesPropability += 1f;
        }

        if (descentEnemiesPropability > 50) {
            descentEnemiesPropability = 80;
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab) {

        float randomPositionX = Random.Range(-horizontalRangeToInstanciate, horizontalRangeToInstanciate);
        Vector3 spawnPosition = new Vector3(randomPositionX, playerTransform.position.y + positionToSpawnInY, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;

        if (distanceFromPlayerToSpawnY > -7f) {
            distanceFromPlayerToSpawnY -= 0.05f;
        }
    }

    private void ShowAlert(GameObject objectToSpawn) {
        float randomPositionX = Random.Range(-horizontalRangeToInstanciate, horizontalRangeToInstanciate);
        Vector3 spawnPosition = new Vector3(randomPositionX, playerTransform.position.y + positionToSpawnInY, 0);
        UIManager.instance.ShowAlertImage(spawnPosition);
        // Invoke(nameof(SpawDescendEnemy), 1.5f);
        StartCoroutine(SpawDescendEnemy(objectToSpawn, spawnPosition));
    }

    private IEnumerator SpawDescendEnemy(GameObject objectToSpawn, Vector3 spawnPosition) {
        yield return new WaitForSeconds(1.5f);
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
