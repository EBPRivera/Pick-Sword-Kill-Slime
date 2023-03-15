using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private HowToPlayUI howToPlayUI;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            Loader.LoadScene(Loader.Scene.GameScene);
        });

        howToPlayButton.onClick.AddListener(() => {
            Hide();
            howToPlayUI.Show();
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
        
        if (HighScoreManager.GetHighScore() >= 0) {
            highScoreText.text += HighScoreManager.GetHighScore().ToString();
            highScoreText.gameObject.SetActive(true);
        }

        Time.timeScale = 1f;
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
