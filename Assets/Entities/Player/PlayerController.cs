using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const string PUSH = "Push";
    private const string ATTACK = "Attack";

    [SerializeField] private float movementSpeed = 150f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private HitboxController HitboxController;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private GameInput gameInput;

    private bool canAct = true;
    private bool isBlinking = false;
    private bool isWalking = false;
    private Rigidbody2D rigidBody;
    private HealthController healthController;
    private Vector2 facingDirection;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        facingDirection = new Vector2(1, 0);
        healthController = GetComponent<HealthController>();
    }

    private void Start() {
        gameInput.OnPushAction += GameInput_OnPushAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
    }

    private void FixedUpdate()
    {
        if (healthController.IsInvulnerable() && !isBlinking) {
            StartCoroutine(TemporaryActionDisableCo());
            StartCoroutine(SpriteBlinkingCo());
        }

        if (gameInput.InputDirection != Vector2.zero && canAct) {
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity + (gameInput.InputDirection * movementSpeed * Time.fixedDeltaTime), maxSpeed);
            facingDirection = gameInput.InputDirection;
            isWalking = true;
        } else {
            isWalking = false;
        }
    }

    private void OnDestroy() {
        gameInput.OnPushAction -= GameInput_OnPushAction;
        gameInput.OnAttackAction -= GameInput_OnAttackAction;
    }

    private void GameInput_OnPushAction(object sender, EventArgs e) {
        ActivateHitBox(PUSH);
    }

    private void GameInput_OnAttackAction(object sender, EventArgs e) {
        ActivateHitBox(ATTACK);
    }

    private void ActivateHitBox(string trigger) {
        if (canAct) {
            canAct = false;
            HitboxController.SetAction(trigger, facingDirection);
            playerAnimator.TriggerAction(trigger);
        }
    }

    private IEnumerator SpriteBlinkingCo() {
        isBlinking = true;

        while (healthController.IsInvulnerable()) {
            playerAnimator.TriggerBlink(false);
            yield return new WaitForSeconds(0.2f);
            playerAnimator.TriggerBlink(true);
            yield return new WaitForSeconds(0.2f);
        }

        isBlinking = true;
    }

    private IEnumerator TemporaryActionDisableCo() {
        canAct = false;
        yield return new WaitForSeconds(0.1f);
        canAct = true;
    }

    public bool IsWalking() {
        return isWalking;
    }

    public void FinishActing() {
        canAct = true;
    }

    public Vector2 GetFacingDirection() {
        return facingDirection;
    }
}
