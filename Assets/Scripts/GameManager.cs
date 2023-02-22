using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    private enum State {
        GamePlay,
        Pause,
        GameOver
    }

    public static GameManager Instance { get; private set; }

    private State state;

    public event EventHandler OnStateChanged;

    private void Awake() {
        Instance = this;
        PauseGame();
    }

    private void Start() {
        GameInput.Instance.OnPauseToggle += GameInput_OnPauseToggle;
        Player.Instance.OnDeath += Player_OnDeath;
    }

    private void OnDestroy() {
        GameInput.Instance.OnPauseToggle -= GameInput_OnPauseToggle;
        Player.Instance.OnDeath -= Player_OnDeath;
    }

    private void GameInput_OnPauseToggle(object sender, EventArgs e) {
        if (state == State.Pause) {
            state = State.GamePlay;
            Time.timeScale = 1f;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        } else if (state == State.GamePlay) {
            PauseGame();
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Player_OnDeath(object sender, EventArgs e) {
        state = State.GameOver;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void PauseGame() {
        state = State.Pause;
        Time.timeScale = 0f;
    }

    public bool IsPlayable() {
        return state == State.GamePlay;
    }

    public bool IsPaused() {
        return state == State.Pause;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }
}
