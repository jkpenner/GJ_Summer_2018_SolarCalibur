using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Temp Assigner: Assigned by Events")]
    public Planet planet;
    private PlanetController planetController;

    private void OnEnable() {
        // Temp Code: If the planet is preassigned then we'll
        // assign it as the player planet
        if (planet != null) {
            SetPlayerPlanet(planet, true);
        }

        if (GameManager.Exists && GameManager.PlayerPlanet != null) {
            OnPlayerAssigned(GameManager.PlayerPlanet);
        }

        if (MsgRelay.Exists) {
            MsgRelay.EventGameStart += OnGameStart;
            MsgRelay.EventGameComplete += OnGameComplete;
            MsgRelay.EventPlayerAssigned += OnPlayerAssigned;
            MsgRelay.EventPlayerUnassigned += OnPlayerUnassigned;
        }
    }

    private void OnDisable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameStart -= OnGameStart;
            MsgRelay.EventGameComplete -= OnGameComplete;
            MsgRelay.EventPlayerAssigned -= OnPlayerAssigned;
            MsgRelay.EventPlayerUnassigned -= OnPlayerUnassigned;
        }
    }

    private void OnGameStart() {

    }

    private void OnGameComplete() {

    }

    private void OnPlayerUnassigned(Planet oldPlanet) {
        if (this.planet == oldPlanet) {
            SetPlayerPlanet(null);
        }
    }

    private void SetPlayerPlanet(Planet new_planet, bool force_assign = false) {
        if (this.planet != new_planet || force_assign) {
            this.planet = new_planet;
            if (this.planet != null) {
                this.planetController = this.planet.GetComponent<PlanetController>();
                if (this.planetController == null) {
                    Debug.LogWarningFormat("[{0}]: Attempting to get PlanetController off {0}, "
                        + "but there is no PlanetController component on the planet.", this.planet.name);
                }
            } else {
                this.planetController = null;
            }
        }
    }

    private void OnPlayerAssigned(Planet newPlanet) {
        SetPlayerPlanet(newPlanet);
    }

    public void Update() {
        if (GameManager.IsGameActive && !GameManager.IsGamePaused && planetController != null) {
            planetController.MoveAroundTarget(Input.GetAxis("Horizontal"));

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
                planetController.StartCharge();
            }

            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) {
                planetController.LaunchAttack();
            }
        }
    }
}
