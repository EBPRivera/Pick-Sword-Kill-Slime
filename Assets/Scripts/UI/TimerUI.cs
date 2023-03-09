using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start() {
        GameManager.Instance.OnTimeChange += GameManager_OnTimeChange;
    }

    private void GameManager_OnTimeChange(object sender, GameManager.OnTimeChangeEventArgs e) {
        timerText.text = "Time: " + e.time.ToString("n0");
    }
}
