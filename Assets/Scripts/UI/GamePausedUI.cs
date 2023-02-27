using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour {

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePause();
        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.LoadScene(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        GameManager.Instance.OnPause += GameManager_OnPause;
        GameManager.Instance.OnUnpause += GameManager_OnUnpause;

        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        GameManager.Instance.OnPause -= GameManager_OnPause;
        GameManager.Instance.OnUnpause -= GameManager_OnUnpause;
    }

    private void GameManager_OnPause(object sender, EventArgs e) {
        gameObject.SetActive(true);
    }

    private void GameManager_OnUnpause(object sender, EventArgs e) {
        gameObject.SetActive(false);
    }
}
