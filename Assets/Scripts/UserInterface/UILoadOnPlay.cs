using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadOnPlay : MonoBehaviour {
    public GameObject[] elementsToLoad;

    private void Awake() {
        foreach (var element in elementsToLoad) {
            var e = Instantiate(element);
            e.transform.SetParent(this.transform, false);
        }
    }
}
