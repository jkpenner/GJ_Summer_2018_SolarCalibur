using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerLost : MonoBehaviour {
    public GameObject content;
    public Button mainMenuBtn;
    public Button retryBtn;

    private void OnEnable() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameComplete += OnGameComplete;
            MsgRelay.EventGameSceneUnloaded += OnGameSceneUnloaded;
        }
        content.gameObject.SetActive(false);

        retryBtn.onClick.AddListener(OnRetryClick);
        mainMenuBtn.onClick.AddListener(OnMainMenuClick);
    }

    private void OnDisable() {
        Debug.Log("Was disabled");
        if (MsgRelay.Exists) {
            MsgRelay.EventGameComplete -= OnGameComplete;
            MsgRelay.EventGameSceneUnloaded -= OnGameSceneUnloaded;
        }
    }

    private void OnGameComplete() {
        if (GameManager.Exists) {
            if (GameManager.PlayerPlanet.IsAlive == false && GameManager.EnemyPlanet.IsAlive) {
                content.gameObject.SetActive(true);
            }
        }
    }

    private void OnMainMenuClick() {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnRetryClick() {
        if (GameManager.Exists) {
            GameManager.RestartCurrentLevel();
        }
    }

    private void OnGameSceneUnloaded() {
        content.gameObject.SetActive(false);
    }
}
