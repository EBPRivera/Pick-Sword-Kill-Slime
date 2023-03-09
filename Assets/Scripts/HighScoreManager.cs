using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {
    private const string HIGH_SCORE = "HighScore";

    public static int GetHighScore() {
        return PlayerPrefs.GetInt(HIGH_SCORE, -1);
    }

    public static void SetHighScore(int score) {
        PlayerPrefs.SetInt(HIGH_SCORE, score);
    }
}
