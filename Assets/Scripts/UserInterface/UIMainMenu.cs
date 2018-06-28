using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour {
    public Button new_game;

    private void OnEnable() {
        new_game.onClick.AddListener(OnNewGameClick);
    }

    private void OnDisable() {
        new_game.onClick.RemoveListener(OnNewGameClick);
    }

    private void OnNewGameClick() {
        if (DataManager.Exists) {
            DataManager.SetToFirstLevel();
        }
        SceneManager.LoadScene("Gameplay");
    }
}
