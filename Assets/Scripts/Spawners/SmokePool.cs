using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokePool : MonoBehaviour {

    [SerializeField] private GameObject prefabToPool;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> pooledObjects;

    public static SmokePool instance;

    void Start() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolSize; i++) {
            GameObject obj = Instantiate(prefabToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetObjectFromPool() {
        if (pooledObjects.Count > 0) {
            GameObject obj = pooledObjects[0];
            pooledObjects.RemoveAt(0);
            obj.SetActive(true);
            // Reset any properties/components if needed
            return obj;
        }
        else {
            GameObject obj = Instantiate(prefabToPool);
            pooledObjects.Add(obj);
            return obj;
        }
    }

    public void ReturnObjectToPool(GameObject obj) {
        obj.SetActive(false);
        pooledObjects.Add(obj);
        // Reset any properties/components if needed
    }
}
