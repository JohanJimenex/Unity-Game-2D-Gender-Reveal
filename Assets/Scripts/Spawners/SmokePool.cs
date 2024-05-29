using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokePool : MonoBehaviour {

    [SerializeField] private GameObject prefabToPool;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pooledObjects;

    public static SmokePool instance;

    void Start() {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        pooledObjects = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++) {
            GameObject obj = Instantiate(prefabToPool);
            obj.SetActive(false);
            pooledObjects.Enqueue(obj);
        }
    }

    public GameObject GetObjectFromPool() {
        if (pooledObjects.Count > 0) {
            GameObject obj = pooledObjects.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else {
            GameObject obj = Instantiate(prefabToPool);
            pooledObjects.Enqueue(obj);
            return obj;
        }
    }

    public void ReturnObjectToPool(GameObject obj) {
        obj.SetActive(false);
        pooledObjects.Enqueue(obj);
        // Reset any properties/components if needed
    }
}
