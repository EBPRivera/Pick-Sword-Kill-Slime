using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private const string PUSH = "Push";
    private const string ATTACK = "Attack";
    private const string DEATH = "Death";

    [SerializeField] private EntitySO entitySO;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask objectLayerMask;

    private bool canAct = true;
    private float pushDetectionDistance = 0.2f;
    private Rigidbody2D rigidBody;
    private HealthController healthController;
    private Collider2D playerCollider;
    public Vector2 FacingDirection { get; private set; }
    public bool IsWalking { get; private set; }

    public event EventHandler<OnTriggerActionEventArgs> OnTriggerAction;
    public class OnTriggerActionEventArgs : EventArgs {
        public string action;
        public Vector2 direction;
    }

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        FacingDirection = Vector2.right;
        healthController = GetComponent<HealthController>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start() {
        gameInput.OnPushAction += GameInput_OnPushAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
        healthController.OnDamage += HealthController_OnDamage;
        playerAnimator.OnFinishActing += PlayerAnimator_OnFinishActing;
    }

    private void FixedUpdate() {
        Vector2 inputDirection = gameInput.GetMovementVectorNormalized();

        if (inputDirection != Vector2.zero && canAct) {
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity + (inputDirection * entitySO.movementSpeed * Time.fixedDeltaTime), entitySO.maxSpeed);
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
        playerAnimator.OnFinishActing -= PlayerAnimator_OnFinishActing;
    }

    private void GameInput_OnPushAction(object sender, EventArgs e) {
        if (canAct) {
            canAct = false;
            TriggerActionEvent(PUSH);
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
            TriggerActionEvent(ATTACK);
        }
    }

    private void HealthController_OnDamage(object sender, EventArgs e) {
        if (healthController.Health <= 0) {
            TriggerActionEvent(DEATH);
            canAct = false;
        } else {
            StartCoroutine(TemporaryActionDisableCo());
        }
    }

    private void PlayerAnimator_OnFinishActing(object sender, EventArgs e) {
        canAct = true;
    }

    private void TriggerActionEvent(string action) {
        OnTriggerAction?.Invoke(this, new OnTriggerActionEventArgs{ action = action, direction = FacingDirection });
    }

    private IEnumerator TemporaryActionDisableCo() {
        canAct = false;
        yield return new WaitForSeconds(0.1f);
        canAct = true;
    }
}
