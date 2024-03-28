using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererController : MonoBehaviour {

    [SerializeField] private Vector2 offset = new Vector2(0, 0);

    [SerializeField] private MeshRenderer meshRenderer;

    void Update() {
        meshRenderer.material.mainTextureOffset = new Vector2(offset.x, offset.y) * Time.time;
    }
}
