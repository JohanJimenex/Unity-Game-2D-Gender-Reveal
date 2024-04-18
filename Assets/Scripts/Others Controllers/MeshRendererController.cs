using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererController : MonoBehaviour {

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector2 offset = new Vector2(0, 0);
    [SerializeField] private MeshRenderer meshRenderer;

    private void Update() {
        MoveMainTextureOffset();
    }

    private void MoveMainTextureOffset() {
        meshRenderer.material.mainTextureOffset += new Vector2(offset.x, offset.y) * Time.deltaTime;
        // meshRenderer.material.mainTextureOffset = new Vector2(offset.x, offset.y) * Time.time;
    }

    private void LateUpdate() {
        MoveGameObjectPosition();
    }

    private void MoveGameObjectPosition() {
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);
    }

}
