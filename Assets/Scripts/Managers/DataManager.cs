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


    private void Awake() {
        if (IsActiveInstance == false) {
            Destroy(this.gameObject);
        } else {
            DontDestroyOnLoad(this.gameObject);
        }

        _levelDatabase = Resources.Load<LevelDBAsset>("LevelDatabase");

        _activeLevelIndex = initialLevelIndex;
    }

    public void LoadCurrentLevel(Action<AsyncOperation> callback) {

    }
}