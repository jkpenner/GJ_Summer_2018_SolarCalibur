using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {
    private bool _isGameActive = false;
    private bool _isGamePaused = false;

    private Planet playerPlanet;
    private Planet enemyPlanet;

    //[Header("Player Spawn Info")]
    //[SerializeField]
    //private Planet playerPlanetPrefab = null;
    //[SerializeField]
    //private Transform playerPlanetSpawn = null;
    //
    //[Header("Enemy Spawn Info")]
    //[SerializeField]
    //private Planet enemyPlanetPrefab = null;
    //[SerializeField]
    //private Transform enemyPlanetSpawn = null;

    #region Static Properties and Methods
    static public LevelAsset InitialLevel { get; set; }

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

    static public void MoveToNextLevel() {
        if (Exists) {
            Instance.InternalMoveToNextLevel();
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
        // Check if there is a data manager in the scene to get level data
        if (DataManager.Exists && DataManager.ActiveLevelAsset != null) {
            // Load the background scene for the planet
            var ao = SceneManager.LoadSceneAsync(
                DataManager.ActiveLevelAsset.sceneName,
                LoadSceneMode.Additive);
            ao.completed += OnPlanetSceneLoaded;
        }
    }

    private void OnPlanetSceneLoaded(AsyncOperation obj) {
        MsgRelay.TriggerGameSceneLoaded();

        SpawnPlanets(DataManager.PlayerPlanetPrefab,
            DataManager.EnemyPlanetPrefab);

        InternalStartGame();
    }

    private void SpawnPlanets(Planet playerPlanetPrefab, Planet enemyPlanetPrefab) {
        if (enemyPlanetPrefab != null) {
            enemyPlanet = Instantiate<Planet>(enemyPlanetPrefab);
            enemyPlanet.transform.SetParent(this.transform);

            // Look for a spawn point for the enemey
            var enemySpawnPoint = GameObject.FindGameObjectWithTag("EnemySpawnPoint");
            if (enemySpawnPoint != null) {
                enemyPlanet.transform.position = enemySpawnPoint.transform.position;
            } else {
                Debug.LogWarning("[GameManager]: No gameobject with tag EnemySpawnPoint found " +
                    "in the planet level secen. Spawning enemy planet at (0, 0, 0).");
                enemyPlanet.transform.position = Vector3.zero;
            }

            enemyPlanet.EventDestroyed += OnPlanetDestroyed;

            if (enemyPlanet != null)
                MsgRelay.TriggerPlanetSpawned(enemyPlanet);
            enemyPlanet.SetPlayerStatus(false);
        } else {
            Debug.LogWarning("[GameManager]: No Enemy Planet Prefab assigned.");
        }

        if (playerPlanetPrefab != null) {
            playerPlanet = Instantiate<Planet>(playerPlanetPrefab);
            playerPlanet.transform.SetParent(this.transform);

            // Look for a spawn point for the player
            var playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            if (playerSpawnPoint != null) {
                playerPlanet.transform.position = playerSpawnPoint.transform.position;
            } else {
                Debug.LogWarning("[GameManager]: No gameobject with tag PlayerSpawnPoint found " +
                    "in the planet level secen. Spawning player planet at (0, 0, 0).");
                playerPlanet.transform.position = Vector3.zero;
            }

            playerPlanet.EventDestroyed += OnPlanetDestroyed;

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

    public void OnPlanetDestroyed(Planet planet) {
        planet.EventDestroyed -= OnPlanetDestroyed;
        Destroy(planet.gameObject);

        InternalCompleteGame();

        // Temp Code: Move To Next should be call by ui
        StartCoroutine(TestMoveToNext());
    }

    public IEnumerator TestMoveToNext() {
        yield return new WaitForSeconds(1f);
        GameManager.MoveToNextLevel();
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

    private void InternalMoveToNextLevel() {
        if (DataManager.Exists) {
            var ao = SceneManager.UnloadSceneAsync(
                DataManager.ActiveLevelAsset.sceneName);
            ao.completed += OnPlanetSceneUnloaded;
        }
    }

    private void OnPlanetSceneUnloaded(AsyncOperation obj) {
        DataManager.MoveToNextLevel();

        if (DataManager.ActiveLevelAsset != null) {
            var ao = SceneManager.LoadSceneAsync(
                DataManager.ActiveLevelAsset.sceneName,
                LoadSceneMode.Additive);
            ao.completed += OnPlanetSceneLoaded;
        } else {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
