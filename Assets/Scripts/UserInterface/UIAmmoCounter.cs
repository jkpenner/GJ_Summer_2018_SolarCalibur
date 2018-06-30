using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAmmoCounter : UIGameplayElement {
    public GameObject[] slots;

    protected override void OnEnable() {
        base.OnEnable();

        if (GameManager.Exists && GameManager.PlayerPlanet != null) {
            OnPlayerAssigned(GameManager.PlayerPlanet);
        }
    }

    protected override void OnPlayerAssigned(Planet planet) {
        base.OnPlayerAssigned(planet);
        Debug.Log("Assigning player planet");

        ActiveController.EventAsteroidSpawned += OnAsteroidSpawned;
        if (ActiveController != null) {
            Debug.Log("fdlkjsdf");
            foreach (var a in ActiveController.Asteroids) {
                Debug.Log("asdfasdfasdf");
                a.EventLaunched += OnAsteroidLaunched;
                a.EventReturned += OnAsteroidReturned;
            }
        }
    }

    protected override void OnPlayerUnassigned(Planet planet) {
        Debug.Log("Unassigning player planet");
        if (ActiveController != null) {
            foreach (var a in ActiveController.Asteroids) {
                a.EventLaunched -= OnAsteroidLaunched;
                a.EventReturned -= OnAsteroidReturned;
            }
        }
        base.OnPlayerUnassigned(planet);
    }

    private void OnAsteroidSpawned(Asteroid a) {
        a.EventLaunched += OnAsteroidLaunched;
        a.EventReturned += OnAsteroidReturned;

        UpdateAsteroidSlots();
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
            slots[i].SetActive(i < ActiveController.ActiveAsteroids);
        }
    }
}
