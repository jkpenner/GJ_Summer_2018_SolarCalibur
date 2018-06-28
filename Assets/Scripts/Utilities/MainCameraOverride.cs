using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraOverride : MonoBehaviour {
    private Camera mainCamera = null;

    private void OnEnable() {
        if (Camera.main != null) {
            mainCamera = Camera.main;
            mainCamera.enabled = false;
        }
    }

    private void OnDisable() {
        if (mainCamera != null) {
            mainCamera.enabled = true;
        }
    }

    void Update() {
        if (mainCamera != null) {
            this.transform.rotation = mainCamera.transform.rotation;
            this.transform.position = mainCamera.transform.position;
        }
    }
}
