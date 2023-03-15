using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            Loader.LoadScene(Loader.Scene.MainMenuScene);
        });

        retryButton.onClick.AddListener(() => {
            Loader.LoadScene(Loader.Scene.GameScene);
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
        int score = GameManager.Instance.GetScore();

        scoreText.text = "Your Score: " + score.ToString();

        if (score > HighScoreManager.GetHighScore()) {
            highScoreText.text = "New High Score!";
        } else {
            highScoreText.text = "High Score: " + HighScoreManager.GetHighScore().ToString();
        }

        gameObject.SetActive(true);
    }
}
