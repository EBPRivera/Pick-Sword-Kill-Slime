using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button mainMenuButton;

    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            Loader.LoadScene(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, EventArgs e) {
        scoreText.text = "Your Score: " + GameManager.Instance.GetScore().ToString();
        gameObject.SetActive(true);
    }
}
