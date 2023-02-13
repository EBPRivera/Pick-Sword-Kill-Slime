using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const string PUSH = "Push";
    private const string ATTACK = "Attack";
    private const string DEATH = "Death";

    [SerializeField] private float movementSpeed = 150f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private HitboxController HitboxController;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private GameInput gameInput;

    private bool canAct = true;
    private Rigidbody2D rigidBody;
    private HealthController healthController;
    public Vector2 FacingDirection { get; private set; }
    public bool IsWalking { get; private set; }

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        FacingDirection = new Vector2(1, 0);
        healthController = GetComponent<HealthController>();
    }

    private void Start() {
        gameInput.OnPushAction += GameInput_OnPushAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
        healthController.OnDamage += HealthController_OnDamage;
    }

    private void FixedUpdate() {
        Vector2 inputDirection = gameInput.GetMovementVectorNormalized();

        if (inputDirection != Vector2.zero && canAct) {
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity + (inputDirection * movementSpeed * Time.fixedDeltaTime), maxSpeed);
            FacingDirection = inputDirection;
            IsWalking = true;
        } else {
            IsWalking = false;
        }
    }

    private void OnDestroy() {
        gameInput.OnPushAction -= GameInput_OnPushAction;
        gameInput.OnAttackAction -= GameInput_OnAttackAction;
        healthController.OnDamage -= HealthController_OnDamage;
    }

    private void GameInput_OnPushAction(object sender, EventArgs e) {
        ActivateHitBox(PUSH);
    }

    private void GameInput_OnAttackAction(object sender, EventArgs e) {
        ActivateHitBox(ATTACK);
    }

    private void HealthController_OnDamage(object sender, EventArgs e) {
        if (healthController.Health <= 0) {
            playerAnimator.TriggerAction(DEATH);
            canAct = false;
        } else {
            StartCoroutine(TemporaryActionDisableCo());
        }
    }

    private void ActivateHitBox(string trigger) {
        if (canAct) {
            canAct = false;
            HitboxController.SetAction(trigger, FacingDirection);
            playerAnimator.TriggerAction(trigger);
        }
    }

    private IEnumerator TemporaryActionDisableCo() {
        canAct = false;
        yield return new WaitForSeconds(0.1f);
        canAct = true;
    }

    public void FinishActing() {
        canAct = true;
    }
}
