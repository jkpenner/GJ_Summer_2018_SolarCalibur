using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIMainMenu : MonoBehaviour {
    public Button new_game;



    private void OnEnable() {
        if (new_game != null)
            new_game.onClick.AddListener(OnNewGameClick);
    }

    private void OnDisable() {
        if (new_game != null)
            new_game.onClick.RemoveListener(OnNewGameClick);
    }

    private void Update() {
        if (Input.anyKeyDown) {
            OnNewGameClick();
        }
    }

    private void OnNewGameClick() {
        if (DataManager.Exists) {
            DataManager.SetToFirstLevel();
        }
        SceneManager.LoadScene("Gameplay");
    }
}
