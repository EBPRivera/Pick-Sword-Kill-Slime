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
    public event EventHandler OnPause;
    public event EventHandler OnUnpause;
    public event EventHandler OnGameOver;

    private void Awake() {
        Instance = this;
        state = State.GamePlay;
    }

    private void Start() {
        GameInput.Instance.OnPauseToggle += GameInput_OnPauseToggle;
        Player.Instance.OnGameOver += Player_OnGameOver;
    }

    private void OnDestroy() {
        GameInput.Instance.OnPauseToggle -= GameInput_OnPauseToggle;
        Player.Instance.OnGameOver -= Player_OnGameOver;
    }

    private void GameInput_OnPauseToggle(object sender, EventArgs e) {
        TogglePause();
    }

    private void Player_OnGameOver(object sender, EventArgs e) {
        state = State.GameOver;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void TogglePause() {
        if (state == State.Pause) {
            state = State.GamePlay;
            Time.timeScale = 1f;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
            OnUnpause?.Invoke(this, EventArgs.Empty);
        } else if (state == State.GamePlay) {
            state = State.Pause;
            Time.timeScale = 0f;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
            OnPause?.Invoke(this, EventArgs.Empty);
        }
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
