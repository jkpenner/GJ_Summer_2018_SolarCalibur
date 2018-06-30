using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyHealth : UIGameplayElement {
    public UIPlayerHealthSegment segmentPrefab;
    private UIPlayerHealthSegment[] segments;
    public Transform segmentParent;
    private Planet enemy = null;

    private void Update() {
        if (enemy != GameManager.EnemyPlanet) {
            enemy = GameManager.EnemyPlanet;
            SetupHealthBarSegmenets(enemy);
        }
    }


    private void SetupHealthBarSegmenets(Planet planet) {
        RemoveHealthBarSegments(planet);

        if (planet == null) return;

        segments = new UIPlayerHealthSegment[planet.MaxHealth];
        for (int i = 0; i < planet.MaxHealth; i++) {
            segments[i] = Instantiate(segmentPrefab);
            segments[i].transform.SetParent(segmentParent, false);
            segments[i].transform.SetSiblingIndex(i);
            segments[i].SetFilled(true, true);
        }
        Debug.Log(segments.Length);

        planet.EventDamaged += OnPlanetDamaged;
    }

    private void RemoveHealthBarSegments(Planet planet) {
        if (planet != null)
            planet.EventDamaged -= OnPlanetDamaged;

        if (segments == null) return;

        for (int i = 0; i < segments.Length; i++) {
            Destroy(segments[i].gameObject);
        }
        segments = null;
    }

    private void OnPlanetDamaged(Planet enemy) {
        for (int i = 0; i < enemy.MaxHealth; i++) {
            segments[i].SetFilled(i < enemy.CurHealth);
        }
    }
}
