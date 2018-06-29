using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DataManager : Singleton<DataManager> {
    public int initialLevelIndex = 0;
    public Planet playerPrefab = null;


    private LevelDBAsset _levelDatabase;

    private int _activeLevelIndex;
    private LevelAsset _activeLevelAsset;



    static public int ActiveLevelIndex {
        get {
            if (Exists) {
                return Instance._activeLevelIndex;
            }
            return -1;
        }
    }

    static public LevelAsset ActiveLevelAsset {
        get {
            if (Exists && Instance._levelDatabase != null) {
                return Instance._levelDatabase.GetLevel(ActiveLevelIndex);
            }
            return null;
        }
    }

    static public Planet PlayerPlanetPrefab {
        get {
            if (Exists) {
                return Instance.playerPrefab;
            }
            return null;
        }
    }

    static public Planet EnemyPlanetPrefab {
        get {
            if (ActiveLevelAsset != null) {
                return ActiveLevelAsset.enemyPrefab;
            }
            return null;
        }
    }

    static public int LevelCount {
        get {
            if (Exists && Instance._levelDatabase != null) {
                return Instance._levelDatabase.levelAssets.Length;
            }
            return 0;
        }
    }

    static public void MoveToNextLevel() {
        if (Exists) {
            Instance._activeLevelIndex += 1;
            // Check if the next level index does not exist
            if (Instance._activeLevelIndex >= Instance._levelDatabase.levelAssets.Length) {
                Instance._activeLevelIndex = -1;
            }
        }
    }

    static public void SetToFirstLevel() {
        if (Exists) {
            Instance._activeLevelIndex = 0;
        }
    }

    private void Awake() {
        if (IsActiveInstance == false) {
            Destroy(this.gameObject);
        } else {
            DontDestroyOnLoad(this.gameObject);
        }

        _levelDatabase = Resources.Load<LevelDBAsset>("LevelDatabase");

        _activeLevelIndex = initialLevelIndex;
    }
}