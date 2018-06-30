using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayElement : MonoBehaviour {
    [SerializeField]
    public GameObject _content;

    protected Planet ActivePlanet { get; private set; }
    protected PlanetController ActiveController { get; private set; }

    protected virtual void OnEnable() {
        this._content.SetActive(false);

        if (MsgRelay.Exists) {
            MsgRelay.EventPlayerAssigned += OnPlayerAssigned;
            MsgRelay.EventPlayerUnassigned += OnPlayerUnassigned;
            MsgRelay.EventGameStart += OnGameStart;
            MsgRelay.EventGameComplete += OnGameComplete;
        }
    }

    protected virtual void OnDisable() {
        this._content.SetActive(false);

        if (MsgRelay.Exists) {
            MsgRelay.EventPlayerAssigned -= OnPlayerAssigned;
            MsgRelay.EventPlayerUnassigned -= OnPlayerUnassigned;
            MsgRelay.EventGameStart -= OnGameStart;
            MsgRelay.EventGameComplete -= OnGameComplete;
        }
    }

    protected virtual void OnGameStart() {
        this._content.SetActive(true);
    }

    protected virtual void OnGameComplete() {
        this._content.SetActive(false);
    }

    protected virtual void OnPlayerAssigned(Planet player) {
        ActivePlanet = player;
        ActiveController = player.GetComponent<PlanetController>();
    }
    protected virtual void OnPlayerUnassigned(Planet player) {
        if (ActivePlanet == player) {
            ActivePlanet = null;
            ActiveController = null;
        }
    }
}
