using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    private enum State {
        GamePlay,
        Pause,
        Interim,
        GameOver
    }

    public static GameManager Instance { get; private set; }

    private State state;
    private float timer = 30f;
    private int score = 0;

    public event EventHandler OnPause;
    public event EventHandler OnUnpause;
    public event EventHandler OnGameOver;
    public event EventHandler<OnScoreChangeEventArgs> OnScoreChange;
    public event EventHandler<OnTimeChangeEventArgs> OnTimeChange;

    public class OnScoreChangeEventArgs: EventArgs {
        public int score;
    }

    public class OnTimeChangeEventArgs: EventArgs {
        public float time;
    }

    private void Awake() {
        Instance = this;
        state = State.GamePlay;
    }

    private void Start() {
        GameInput.Instance.OnPauseToggle += GameInput_OnPauseToggle;
        Player.Instance.OnDeath += Player_OnDeath;
        Player.Instance.OnGameOver += Player_OnGameOver;
        Enemy.OnAnyDeath += Enemy_OnAnyDeath;
    }

    private void Update() {
        switch (state) {
            case State.GamePlay:
                timer -= Time.deltaTime;
                OnTimeChange?.Invoke(this, new OnTimeChangeEventArgs { time = timer });

                if (timer <= 0) {
                    TriggerGameOver();
                }
                break;
            default:
                break;
        }
    }

    private void OnDestroy() {
        GameInput.Instance.OnPauseToggle -= GameInput_OnPauseToggle;
        Player.Instance.OnDeath -= Player_OnDeath;
        Player.Instance.OnGameOver -= Player_OnGameOver;
        Enemy.OnAnyDeath -= Enemy_OnAnyDeath;

        Enemy.ResetStaticData();
    }

    private void GameInput_OnPauseToggle(object sender, EventArgs e) {
        TogglePause();
    }

    private void Player_OnDeath(object sender, EventArgs e) {
        state = State.Interim;
    }

    private void Player_OnGameOver(object sender, EventArgs e) {
        TriggerGameOver();
    }

    private void Enemy_OnAnyDeath(object sender, EventArgs e) {
        score++;
        OnScoreChange?.Invoke(this, new OnScoreChangeEventArgs { score = score });
    }

    private void TriggerGameOver() {
        state = State.GameOver;
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void TogglePause() {
        if (state == State.Pause) {
            state = State.GamePlay;
            Time.timeScale = 1f;
            OnUnpause?.Invoke(this, EventArgs.Empty);
        } else if (state == State.GamePlay) {
            state = State.Pause;
            Time.timeScale = 0f;
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
