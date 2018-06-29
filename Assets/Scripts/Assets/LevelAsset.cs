using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
[System.Serializable]
public class LevelAsset : ScriptableObject {
    public string sceneName = "";
    public Planet enemyPrefab = null;
}
