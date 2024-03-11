using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesSpawner : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject livePrefab;
    public Transform playerTransform;

    public float spawnHeight = 10f;
    public float horizontalRangeToMove = 2.5f;
    public float distanceToSpawn = -2f;

    private float lastSpawnPositionY;
    private float timeCounter = 0f;

    void Update() {
        timeCounter += Time.deltaTime;

        if (ShouldSpawn() && timeCounter >= 40f) {
            SpawnLive();
            timeCounter = 0f;
        }
    }

    private bool ShouldSpawn() {
        return (playerTransform.position.y - lastSpawnPositionY) >= distanceToSpawn;
    }

    private void SpawnLive() {
        float randomPositionX = Random.Range(-horizontalRangeToMove, horizontalRangeToMove);
        Vector3 spawnPosition = new Vector3(randomPositionX, playerTransform.position.y + spawnHeight, 0);
        Instantiate(livePrefab, spawnPosition, Quaternion.identity);
        lastSpawnPositionY = spawnPosition.y;

        if (distanceToSpawn < 4f) {
            distanceToSpawn -= 0.1f;
        }
    }
}
