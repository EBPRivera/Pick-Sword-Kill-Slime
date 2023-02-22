using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateUI : MonoBehaviour {
    private const string GAME_OVER_TEXT = "Game Over";
    private const string PAUSED_GAME_TEXT = "Paused";

    [SerializeField] private TextMeshProUGUI stateText;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        DisplayText();
    }

    private void OnDestroy() {
        GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        DisplayText();
    }

    private void DisplayText() {
        if (GameManager.Instance.IsPaused()) {
            stateText.text = PAUSED_GAME_TEXT;
            Show();
        } else if (GameManager.Instance.IsGameOver()) {
            stateText.text = GAME_OVER_TEXT;
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
