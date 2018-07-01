using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerWon : MonoBehaviour {
    public GameObject content;
    public Button nextLevelBtn;

    private void OnEnable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameComplete += OnGameComplete;
            MsgRelay.EventGameSceneUnloaded += OnGameSceneUnloaded;
        }

        nextLevelBtn.onClick.AddListener(OnNextLevelClick);

        content.gameObject.SetActive(false);
    }

    private void OnDisable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameComplete -= OnGameComplete;
            MsgRelay.EventGameSceneUnloaded -= OnGameSceneUnloaded;
        }

        nextLevelBtn.onClick.RemoveListener(OnNextLevelClick);
    }

    private void Update() {
        if (content.activeSelf && Input.anyKeyDown) {
            OnNextLevelClick();
        }
    }

    private void OnGameComplete() {
        if (GameManager.Exists) {
            if (GameManager.PlayerPlanet.IsAlive && GameManager.EnemyPlanet.IsAlive == false) {
                content.gameObject.SetActive(true);
            }
        }
    }

    private void OnNextLevelClick() {
        if (GameManager.Exists) {
            GameManager.MoveToNextLevel();
        }
    }

    private void OnGameSceneUnloaded() {
        content.gameObject.SetActive(false);
    }
}
