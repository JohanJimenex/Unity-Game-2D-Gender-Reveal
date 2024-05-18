using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlanetsSpawner : MonoBehaviour {

    [Header("Dependecies")]
    [SerializeField] private GameObject CameraGameObject;
    [SerializeField] private List<GameObject> prefabPlanets;

    [Header("Settings")]
    [SerializeField] private float timeToSpawn = 0;

    private float timer = 0f;
    private float lastSpawnPosition = 0f;
    private List<GameObject> prefabPlanetBackUp;

    private void Start() {
        prefabPlanetBackUp = prefabPlanets;
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer >= timeToSpawn && CameraGameObject.transform.position.y > lastSpawnPosition) {
            SpawnItem();
            timer = 0f;
        }
    }

    private void SpawnItem() {

        int randomIndex = Random.Range(0, prefabPlanets.Count);

        GameObject randomGameObject = prefabPlanets[randomIndex];
        prefabPlanets.RemoveAt(randomIndex);

        Vector3 spawnPosition = new Vector3(1.5f, CameraGameObject.transform.position.y + 6f, 0);

        Instantiate(randomGameObject, spawnPosition, Quaternion.identity, CameraGameObject.transform);

        // spawnedObject.transform.SetParent(CameraGameObject.transform);

        lastSpawnPosition = spawnPosition.y + 10;

        if (prefabPlanets.Count == 0) {
            prefabPlanets = prefabPlanetBackUp;
        }
    }


}
