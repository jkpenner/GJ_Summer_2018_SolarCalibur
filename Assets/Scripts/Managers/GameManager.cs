using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    private bool _isGameActive = false;
    private bool _isGamePaused = false;

    private Planet playerPlanet;
    private Planet enemyPlanet;

    [Header("Player Spawn Info")]
    [SerializeField]
    private Planet playerPlanetPrefab;
    [SerializeField]
    private Transform playerPlanetSpawn;

    [Header("Enemy Spawn Info")]
    [SerializeField]
    private Planet enemyPlanetPrefab;
    [SerializeField]
    private Transform enemyPlanetSpawn;

    #region Static Properties and Methods
    static public Planet PlayerPlanet {
        get {
            if (Exists) {
                return Instance.playerPlanet;
            }
            return null;
        }
    }

    static public Planet EnemyPlanet {
        get {
            if (Exists) {
                return Instance.enemyPlanet;
            }
            return null;
        }
    }

    static public bool IsGamePaused {
        get {
            if (Exists) {
                return Instance._isGamePaused;
            }
            return false;
        }
    }

    static public bool IsGameActive {
        get {
            if (Exists) {
                return Instance._isGameActive;
            }
            return false;
        }
    }

    static public void StartGame() {
        if (Exists) {
            Instance.InternalStartGame();
        }
    }

    static public void CompleteGame() {
        if (Exists) {
            Instance.InternalCompleteGame();
        }
    }

    static public void TogglePaused() {
        if (Exists) {
            Instance.InternalTogglePause();
        }
    }
    #endregion

    private void Awake() {
        if (this.IsActiveInstance == false) {
            Debug.LogFormat("[{0}]: Another instance of MsgRelay is already in the scene. " +
                "Destroying additional copy.", this.name);
            Destroy(this.gameObject);
        }
    }

    public void Start() {
        SpawnPlanets();

        InternalStartGame();
    }

    private void SpawnPlanets() {
        if (enemyPlanetPrefab != null) {
            enemyPlanet = Instantiate<Planet>(enemyPlanetPrefab);
            enemyPlanet.transform.SetParent(this.transform);
            enemyPlanet.transform.position = enemyPlanetSpawn.position;

            if (enemyPlanet != null)
                MsgRelay.TriggerPlanetSpawned(enemyPlanet);
            enemyPlanet.SetPlayerStatus(false);
        } else {
            Debug.LogWarning("[GameManager]: No Enemy Planet Prefab assigned.");
        }

        if (playerPlanetPrefab != null) {
            playerPlanet = Instantiate<Planet>(playerPlanetPrefab);
            playerPlanet.transform.SetParent(this.transform);
            playerPlanet.transform.position = playerPlanetSpawn.position;

            if (playerPlanet != null)
                MsgRelay.TriggerPlanetSpawned(playerPlanet);
            playerPlanet.SetPlayerStatus(true, true);
        } else {
            Debug.LogWarning("[GameManager]: No Enemy Planet Prefab assigned.");
        }

        if (enemyPlanet != null && playerPlanet != null) {
            Debug.Log("Setting up targets");
            playerPlanet.SetTargetPlanet(enemyPlanet);
            enemyPlanet.SetTargetPlanet(playerPlanet);
        }
    }

    private void InternalStartGame() {
        if (_isGameActive == false) {
            _isGameActive = true;
            MsgRelay.TriggerGameStart();
        } else {
            Debug.LogWarningFormat("[{0}]: Attempted to start game, " +
                "but the game is already active.", this.name);
        }
    }

    private void InternalCompleteGame() {
        if (_isGameActive == true) {
            _isGameActive = false;
            MsgRelay.TriggerGameComplete();
        } else {
            Debug.LogWarningFormat("[{0}]: Attempted to complete game, " +
                "but the game was not active.", this.name);
        }
    }

    private void InternalTogglePause() {
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
            MsgRelay.TriggerGamePause();
        else
            MsgRelay.TriggerGameResume();
    }
}
