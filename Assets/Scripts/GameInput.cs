using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnPushAction;
    public event EventHandler OnAttackAction;

    private PlayerInputActions playerInputActions;

    public void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Push.performed += Push_performed;
        playerInputActions.Player.Attack.performed += Attack_performed;
    }

    public void OnDestory() {
        playerInputActions.Player.Push.performed -= Push_performed;
        playerInputActions.Player.Attack.performed -= Attack_performed;
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputDirection = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputDirection.Normalize();

        return inputDirection;
    }

    private void Push_performed(InputAction.CallbackContext obj) {
        OnPushAction?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(InputAction.CallbackContext obj){
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }
}
