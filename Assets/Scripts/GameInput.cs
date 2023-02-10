using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public Vector2 InputDirection { get; private set; }
    public event EventHandler OnPushAction;
    public event EventHandler OnAttackAction;

    private void OnMove(InputValue input) {
        InputDirection = input.Get<Vector2>();
    }

    private void OnPush() {
        OnPushAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnAttack() {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }
}
