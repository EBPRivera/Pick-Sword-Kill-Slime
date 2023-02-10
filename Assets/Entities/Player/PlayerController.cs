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

    private bool canAct = true;
    private bool isBlinking = false;
    private bool isWalking = false;
    private Vector2 inputDirection;
    private Rigidbody2D rigidBody;
    private HealthController healthController;
    private Vector2 facingDirection;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        facingDirection = new Vector2(1, 0);
        healthController = GetComponent<HealthController>();
    }

    private void FixedUpdate()
    {
        if (healthController.IsInvulnerable() && !isBlinking) {
            StartCoroutine(TemporaryActionDisableCo());
            StartCoroutine(SpriteBlinkingCo());
        }

        if (inputDirection != Vector2.zero && canAct) {
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity + (inputDirection * movementSpeed * Time.fixedDeltaTime), maxSpeed);
            facingDirection = inputDirection;
            isWalking = true;
        } else {
            isWalking = false;
        }
    }

    private void OnMove(InputValue input) {
        inputDirection = input.Get<Vector2>();
    }

    private void OnPush() {
        ActivateHitBox(PUSH);
    }

    private void OnAttack() {
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
