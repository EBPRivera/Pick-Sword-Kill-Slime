using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    public event EventHandler OnPushAction;
    public event EventHandler OnAttackAction;
    public event EventHandler OnPauseToggle;

    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Push.performed += Push_performed;
        playerInputActions.Player.Attack.performed += Attack_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        playerInputActions.Player.Push.performed -= Push_performed;
        playerInputActions.Player.Attack.performed -= Attack_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputDirection = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputDirection.Normalize();

        return inputDirection;
    }

    private void Push_performed(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsPlayable()) return;

        OnPushAction?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(InputAction.CallbackContext obj){
        if (!GameManager.Instance.IsPlayable()) return;

        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(InputAction.CallbackContext obj) {
        OnPauseToggle?.Invoke(this, EventArgs.Empty);
    }
}
