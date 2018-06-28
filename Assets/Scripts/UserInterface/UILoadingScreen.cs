using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingScreen : MonoBehaviour {
    private void OnEnable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameStart += OnGameStart;
            MsgRelay.EventGameComplete += OnGameComplete;
        }
    }

    private void OnDisable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameStart -= OnGameStart;
            MsgRelay.EventGameComplete -= OnGameComplete;
        }
    }

    private void OnGameStart() {
        this.gameObject.SetActive(false);
    }

    public void OnGameComplete() {
        this.gameObject.SetActive(true);
    }
}
