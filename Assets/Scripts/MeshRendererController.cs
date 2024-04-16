using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererController : MonoBehaviour {

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector2 offset = new Vector2(0, 0);
    [SerializeField] private MeshRenderer meshRenderer;

    private void LateUpdate() {
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);
    }

    private void Update() {
        meshRenderer.material.mainTextureOffset = new Vector2(offset.x, offset.y) * Time.time;
    }

}
