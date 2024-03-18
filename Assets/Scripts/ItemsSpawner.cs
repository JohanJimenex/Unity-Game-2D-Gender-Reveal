using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour {

    [SerializeField] private Transform playerController;
    [SerializeField] private GameObject[] prefab;
    [SerializeField] private float timeToSpawn = 30f;

    private float timer = 0f;
    private float lastSpawnPosition = 0f;

    void Update() {
        timer += Time.deltaTime;

        if (timer >= timeToSpawn && playerController.position.y > lastSpawnPosition) {
            SpawnItem();
            timer = 0f;
        }
    }

    private void SpawnItem() {

        GameObject randomGameObject = prefab[Random.Range(0, prefab.Length)];

        Vector3 spawnPosition = new Vector3(Random.Range(-3, 3), playerController.position.y + 6f, 0);
        Instantiate(randomGameObject, spawnPosition, Quaternion.identity);
        lastSpawnPosition = spawnPosition.y + 10;
    }


}
