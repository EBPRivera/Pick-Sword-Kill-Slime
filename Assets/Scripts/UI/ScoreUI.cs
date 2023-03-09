using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;
    
    private void Start() {
        GameManager.Instance.OnScoreChange += GameManager_OnScoreChange;
    }

    private void GameManager_OnScoreChange(object sender, GameManager.OnScoreChangeEventArgs e) {
        scoreText.text = "Score: " + e.score.ToString();
    }
}
