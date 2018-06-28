using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public float followRate = 1f;
    [Header("Temp Assigner: Assigned by Events")]
    public Transform target;

    public void OnEnable() {
        // Check if there is already a player planet assigned when enabled
        if (GameManager.Exists && GameManager.PlayerPlanet != null) {
            OnPlayerAssigned(GameManager.PlayerPlanet);
        }

        // Add listeners to required events
        if (MsgRelay.Exists) {
            MsgRelay.EventPlayerAssigned += OnPlayerAssigned;
            MsgRelay.EventPlayerUnassigned += OnPlayerUnassigned;
        }
    }

    public void OnDisable() {
        // If msgrelay still exists then remove listeners
        if (MsgRelay.Exists) {
            MsgRelay.EventPlayerAssigned -= OnPlayerAssigned;
            MsgRelay.EventPlayerUnassigned -= OnPlayerUnassigned;
        }
    }

    private void OnPlayerAssigned(Planet planet) {
        target = planet.transform;
    }

    private void OnPlayerUnassigned(Planet planet) {
        if (target == planet.transform)
            target = null;
    }

    public void Update() {
        if (target != null) {
            transform.position = Vector3.Lerp(transform.position,
                target.position, followRate * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                target.rotation, 0.4f);
        }
    }
}
