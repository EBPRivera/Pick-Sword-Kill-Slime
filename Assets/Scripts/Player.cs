using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    private const string PUSH = "Push";
    private const string ATTACK = "Attack";
    private const string DEATH = "Death";

    public static Player Instance;

    [SerializeField] private EntitySO entitySO;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask objectLayerMask;
    [SerializeField] private PickableListSO pickableWeapons;

    private float health;
    private bool isInvulnerable;
    private bool canAct = true;
    private float pushDetectionDistance = 0.2f;
    private float invulnerableTimer = 1f;
    private string currentAttackAction;
    private Dictionary<string, int> attackCountDictionary = new Dictionary<string, int>();
    private Rigidbody2D rigidBody;
    private Collider2D playerCollider;
    public Vector2 FacingDirection { get; private set; }
    public bool IsWalking { get; private set; }

    public event EventHandler<OnTriggerActionEventArgs> OnTriggerAction;
    public event EventHandler OnDeath;
    public event EventHandler<OnInvulnerableToggleEventArgs> OnInvulnerableToggle;
    public event EventHandler<OnWeaponPickupEventArgs> OnWeaponPickup;
    public class OnWeaponPickupEventArgs: EventArgs {
        public PickableSO pickedWeapon;
        public int weaponCount;
    }
    public class OnTriggerActionEventArgs : EventArgs {
        public string action;
    }
    public class OnInvulnerableToggleEventArgs : EventArgs {
        public bool isInvulnerable;
        public float health;
    }

    private void Awake() {
        Instance = this;

        health = entitySO.health;
        isInvulnerable = false;
        rigidBody = GetComponent<Rigidbody2D>();
        FacingDirection = Vector2.right;
        playerCollider = GetComponent<Collider2D>();

        foreach (PickableSO pickableWeapon in pickableWeapons.pickableSOList) {
            attackCountDictionary.Add(pickableWeapon.pickableObjectName, 0);
            if (currentAttackAction == null) {
                currentAttackAction = pickableWeapon.pickableObjectName;
            }
        }
    }

    private void Start() {
        gameInput.OnPushAction += GameInput_OnPushAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
        playerAnimator.OnFinishActing += PlayerAnimator_OnFinishActing;
        playerAnimator.OnDeath += PlayerAnimator_OnDeath;
    }

    private void FixedUpdate() {
        Vector2 inputDirection = gameInput.GetMovementVectorNormalized();

        if (inputDirection != Vector2.zero && canAct) {
            rigidBody.velocity = rigidBody.velocity + (inputDirection * entitySO.movementSpeed * Time.fixedDeltaTime);
            FacingDirection = inputDirection;
            IsWalking = true;
        } else {
            IsWalking = false;
        }
    }

    private void OnDestroy() {
        gameInput.OnPushAction -= GameInput_OnPushAction;
        gameInput.OnAttackAction -= GameInput_OnAttackAction;
        playerAnimator.OnFinishActing -= PlayerAnimator_OnFinishActing;
        playerAnimator.OnDeath -= PlayerAnimator_OnDeath;
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
        if (canAct && attackCountDictionary[currentAttackAction] > 0) {
            canAct = false;
            TriggerActionEvent(ATTACK);
            attackCountDictionary[currentAttackAction]--;
        }
    }

    private void PlayerAnimator_OnFinishActing(object sender, EventArgs e) {
        canAct = true;
    }

    private void PlayerAnimator_OnDeath(object sender, EventArgs e) {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage(float damage, Vector2 knockback) {
        if (!isInvulnerable) {
            health -= damage;
            rigidBody.AddForce(knockback, ForceMode2D.Impulse);

            if (health <= 0) {
                canAct = false;
                isInvulnerable = true;
                TriggerActionEvent(DEATH);
            } else {
                StartCoroutine(InvulnerableStateCo());
                StartCoroutine(TemporaryActionDisableCo());
            }
        }
    }

    private void TriggerActionEvent(string action) {
        OnTriggerAction?.Invoke(this, new OnTriggerActionEventArgs{ action = action });
    }

    private IEnumerator InvulnerableStateCo() {
        if (invulnerableTimer > 0) {
            isInvulnerable = true;
            OnInvulnerableToggle?.Invoke(this, new OnInvulnerableToggleEventArgs{ isInvulnerable = true, health = health });
            yield return new WaitForSeconds(invulnerableTimer);
            isInvulnerable = false;
            OnInvulnerableToggle?.Invoke(this, new OnInvulnerableToggleEventArgs{ isInvulnerable = false, health = health });
        }
    }

    private IEnumerator TemporaryActionDisableCo() {
        canAct = false;
        yield return new WaitForSeconds(0.1f);
        canAct = true;
    }

    public bool IsAlive() {
        return health > 0;
    }

    public EntitySO GetEntitySO() {
        return entitySO;
    }

    public void ItemPickup(PickableSO pickedObject) {
        // Check if picked object is a weapon
        foreach (PickableSO pickableWeapon in pickableWeapons.pickableSOList) {
            if (pickedObject == pickableWeapon) {
                attackCountDictionary[pickableWeapon.pickableObjectName]++;
                OnWeaponPickup?.Invoke(this, new OnWeaponPickupEventArgs{
                    pickedWeapon = pickedObject,
                    weaponCount = attackCountDictionary[pickableWeapon.pickableObjectName]
                });
                break;
            }
        }
    }
}
