using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAmmoCounter : MonoBehaviour {
    public GameObject content;
    public GameObject[] slots;

    private Planet activePlanet;
    private PlanetController activeController;

    private void OnEnable() {
        if (GameManager.Exists && GameManager.PlayerPlanet != null) {
            content.SetActive(GameManager.IsGameActive);
            OnPlayerAssigned(GameManager.PlayerPlanet);
        }

        if (MsgRelay.Exists) {
            MsgRelay.EventPlayerAssigned += OnPlayerAssigned;
            MsgRelay.EventPlayerUnassigned += OnPlayerUnassigned;
            MsgRelay.EventGameStart += OnGameStart;
            MsgRelay.EventGameComplete += OnGameComplete;
        }
    }

    private void OnDisable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventPlayerAssigned -= OnPlayerAssigned;
            MsgRelay.EventPlayerUnassigned -= OnPlayerUnassigned;
        }
    }

    private void OnGameStart() {
        content.SetActive(true);
    }

    private void OnGameComplete() {
        content.SetActive(false);
    }

    private void OnPlayerAssigned(Planet planet) {
        Debug.Log("Assigning player planet");
        activePlanet = planet;
        activeController = planet.GetComponent<PlanetController>();
        activeController.EventAsteroidSpawned += OnAsteroidSpawned;

        if (activeController != null) {
            Debug.Log("fdlkjsdf");
            foreach (var a in activeController.Asteroids) {
                Debug.Log("asdfasdfasdf");
                a.EventLaunched += OnAsteroidLaunched;
                a.EventReturned += OnAsteroidReturned;
            }
        }
    }

    private void OnAsteroidSpawned(Asteroid a) {
        a.EventLaunched += OnAsteroidLaunched;
        a.EventReturned += OnAsteroidReturned;

        UpdateAsteroidSlots();
    }

    private void OnPlayerUnassigned(Planet planet) {
        Debug.Log("Unassigning player planet");
        if (activeController != null) {
            foreach (var a in activeController.Asteroids) {
                a.EventLaunched -= OnAsteroidLaunched;
                a.EventReturned -= OnAsteroidReturned;
            }
            activeController = null;
        }
        activePlanet = null;
    }

    private void OnAsteroidLaunched(Asteroid obj) {
        Debug.Log("Asteroid Launched");
        UpdateAsteroidSlots();
    }

    private void OnAsteroidReturned(Asteroid obj) {
        Debug.Log("Asteroid Returned");
        UpdateAsteroidSlots();
    }

    private void UpdateAsteroidSlots() {
        for (int i = 0; i < slots.Length; i++) {
            slots[i].SetActive(i < activeController.ActiveAsteroids);
        }
    }
}
