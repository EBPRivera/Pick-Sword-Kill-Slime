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

    [Header("Player Attributes")]
    [SerializeField] private float movementSpeed = 150f;
    [SerializeField] private float maxSpeed = 8f;
    
    [Header("Children")]
    [SerializeField] private HitboxController hitboxController;
    [SerializeField] private PlayerAnimator playerAnimator;
    
    [Header("Game Input")]
    [SerializeField] private GameInput gameInput;

    [Header("Layer Selection")]
    [SerializeField] private LayerMask objectLayerMask;

    private bool canAct = true;
    private float pushDetectionDistance = 0.2f;
    private Rigidbody2D rigidBody;
    private HealthController healthController;
    private Collider2D playerCollider;
    public Vector2 FacingDirection { get; private set; }
    public bool IsWalking { get; private set; }

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        FacingDirection = new Vector2(1, 0);
        healthController = GetComponent<HealthController>();
        playerCollider = GetComponent<Collider2D>();
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
        if (canAct) {
            canAct = false;
            playerAnimator.TriggerAction(PUSH);
            Vector2 startPosition = playerCollider.ClosestPoint((Vector2) gameObject.transform.position + FacingDirection);
            RaycastHit2D raycastHit = Physics2D.Raycast(startPosition, FacingDirection, pushDetectionDistance, objectLayerMask);
            
            if (raycastHit) {
                raycastHit.transform.gameObject.TryGetComponent<IPushable>(out IPushable pushableObject);
                pushableObject?.Push(gameObject.transform.position);
            }
        }
    }

    private void GameInput_OnAttackAction(object sender, EventArgs e) {
        if (canAct) {
            canAct = false;
            hitboxController.SetAction(ATTACK, FacingDirection);
            playerAnimator.TriggerAction(ATTACK);
        }
    }

    private void HealthController_OnDamage(object sender, EventArgs e) {
        if (healthController.Health <= 0) {
            playerAnimator.TriggerAction(DEATH);
            canAct = false;
        } else {
            StartCoroutine(TemporaryActionDisableCo());
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
