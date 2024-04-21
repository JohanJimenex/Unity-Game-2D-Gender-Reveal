using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlanetsSpawner : MonoBehaviour {

    [Header("Dependecies")]
    [SerializeField] private GameObject CameraGameObject;
    [SerializeField] private GameObject[] prefabPlanets;

    [Header("Settings")]
    [SerializeField] private float timeToSpawn = 0;

    private float timer = 0f;
    private float lastSpawnPosition = 0f;

    void Update() {
        timer += Time.deltaTime;

        if (timer >= timeToSpawn && CameraGameObject.transform.position.y > lastSpawnPosition) {
            SpawnItem();
            timer = 0f;
        }
    }

    private void SpawnItem() {

        GameObject randomGameObject = prefabPlanets[Random.Range(0, prefabPlanets.Length)];

        Vector3 spawnPosition = new Vector3(1.5f, CameraGameObject.transform.position.y + 6f, 0);

        GameObject spawnedObject = Instantiate(randomGameObject, spawnPosition, Quaternion.identity);

        spawnedObject.transform.SetParent(CameraGameObject.transform);

        lastSpawnPosition = spawnPosition.y + 10;
    }


}
