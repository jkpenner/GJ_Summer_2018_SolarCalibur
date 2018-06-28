using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingScreen : MonoBehaviour {
    public GameObject content;

    private void OnEnable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameSceneLoaded += OnGameSceneLoaded;
            MsgRelay.EventGameSceneUnloaded += OnGameSceneUnloaded;
        }
    }

    private void OnDisable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameStart -= OnGameSceneLoaded;
            MsgRelay.EventGameComplete -= OnGameSceneUnloaded;
        }
    }

    private void OnGameSceneLoaded() {
        content.gameObject.SetActive(false);
    }

    public void OnGameSceneUnloaded() {
        content.gameObject.SetActive(true);
    }
}
