using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
[System.Serializable]
public class LevelDBAsset : ScriptableObject {
    public LevelAsset[] levelAssets;

    public LevelAsset GetLevel(int index) {
        if (index >= 0 && index < levelAssets.Length) {
            return levelAssets[index];
        }
        return null;
    }
}
