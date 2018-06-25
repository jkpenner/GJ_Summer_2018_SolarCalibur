using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public Transform target;
    public float followRate = 1f;

    public void Update() {
        transform.position = Vector3.Lerp(transform.position,
            target.position, followRate * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            target.rotation, 0.4f);
    }
}
