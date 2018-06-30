using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

    public int cometFrequency;

    private GameObject prefab, spaceObjectContainer;

    public int spaceObjectCount = 1000;

    public ObjectInfo[] objectInfos;

    // Use this for initialization
    void Start() {
        prefab = Resources.Load("space object") as GameObject;
        spaceObjectContainer = new GameObject("space objects");

        foreach (var oi in objectInfos) {
            if (oi.count > 0) {
                for (int i = 0; i < oi.count; i++) {
                    MakeSpaceObject(oi);
                }
            }
        }

        for (int i = 0; i < spaceObjectCount; i++) {
            MakeSpaceObject(GetRandomObjectInfo());
        }
    }

    private void OnDestroy() {
        Destroy(spaceObjectContainer);
    }

    private ObjectInfo GetRandomObjectInfo() {
        int totalWeight = 0;
        foreach (var oi in objectInfos) {
            totalWeight += oi.randomWeight;
        }

        int r = Random.Range(0, totalWeight);
        int indexSoFar = 0;
        foreach (var oi in objectInfos) {
            indexSoFar += oi.randomWeight;
            if (r < indexSoFar) {
                return oi;
            }
        }
        return null;
    }


    private void MakeSpaceObject(ObjectInfo oi) {
        Vector3 position = Random.onUnitSphere * (10 + oi.depth);
        GameObject newSpaceObject = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        newSpaceObject.transform.parent = spaceObjectContainer.transform;
        newSpaceObject.transform.LookAt(Vector3.zero);
        var mat = newSpaceObject.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        mat.SetTexture("_MainTex", oi.texture);
        Color newColor = Color.Lerp(oi.color1, oi.color2, Random.Range(0f, 1f));
        newColor.a = 1;
        mat.color = newColor;

        float newSize = Random.Range(oi.minSize, oi.maxSize);
        newSpaceObject.transform.localScale = new Vector3(newSize, newSize, newSize);
    }

    // Update is called once per frame
    void Update() {
        if (!GameManager.IsGamePaused && GameManager.PlayerPlanet != null) {
            spaceObjectContainer.transform.position = GameManager.PlayerPlanet.transform.position;
        }
    }

    [System.Serializable]
    public class ObjectInfo {
        public Texture texture;
        public int randomWeight;
        public int count;
        public float minSize, maxSize;
        public Color color1, color2;
        public int depth;
    }
}
