using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour {
    public Vector3 speeds;

    private void Update() {
        transform.Rotate(speeds * Time.deltaTime);
    }
}
