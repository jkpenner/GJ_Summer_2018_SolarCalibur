using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public PlanetController planet;

    private void OnEnable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameStart += OnGameStart;
            MsgRelay.EventGameComplete += OnGameComplete;
        }
    }

    private void OnDisable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameStart -= OnGameStart;
            MsgRelay.EventGameComplete -= OnGameComplete;
        }
    }

    private void OnGameStart() {

    }

    private void OnGameComplete() {

    }

    public void Update() {
        if (GameManager.IsGameActive && !GameManager.IsGamePaused && planet != null) {
            planet.MoveAroundTarget(Input.GetAxis("Horizontal"));

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
                planet.StartCharge();
            }

            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) {
                planet.LaunchAttack();
            }
        }
    }
}
