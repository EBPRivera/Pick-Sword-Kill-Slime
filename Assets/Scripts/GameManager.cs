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

    private void Awake() {
        Instance = this;
        state = State.Pause;
    }

    private void Start() {
        GameInput.Instance.OnPauseToggle += GameInput_OnPausetoggle;
    }

    private void Update() {
        switch(state) {
            case State.GamePlay:
                break;
            case State.Pause:
                break;
            case State.GameOver:
                break;
        }
    }

    private void GameInput_OnPausetoggle(object sender, EventArgs e) {
        if (state == State.Pause) {
            state = State.GamePlay;
        } else if (state == State.GamePlay) {
            state = State.Pause;
        }
    }

    public bool IsPlayable() {
        return state == State.GamePlay;
    }
}
