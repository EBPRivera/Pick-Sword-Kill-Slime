using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable {
    
    public static Player Instance;

    [SerializeField] private EntitySO entitySO;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private GameInput gameInput;

    [Header("Pickable Objects")]
    [SerializeField] private PickableListSO pickableWeapons;
    [SerializeField] private PickableListSO pickableHealingItems;

    private float health;
    private float maxHealth;
    private bool isInvulnerable;
    private bool canAct = true;
    private float invulnerableTimer = 1f;
    private PickableSO currentAttackAction;
    private Dictionary<PickableSO, int> attackCountDictionary = new Dictionary<PickableSO, int>();
    private Rigidbody2D rigidBody;
    public Vector2 FacingDirection { get; private set; }
    public bool IsWalking { get; private set; }

    public event EventHandler OnDamaged;
    public event EventHandler OnDeath;
    public event EventHandler OnGameOver;
    public event EventHandler<OnAttackActionEventArgs> OnAttackAction;
    public event EventHandler<OnInvulnerableToggleEventArgs> OnInvulnerableToggle;
    public event EventHandler<OnWeaponPickupEventArgs> OnWeaponPickup;
    public event EventHandler OnHealthPickup;
    public event EventHandler<IDamageable.OnHealthChangeEventArgs> OnHealthChange;

    public class OnWeaponPickupEventArgs: EventArgs {
        public PickableSO pickedWeapon;
        public int weaponCount;
    }
    public class OnInvulnerableToggleEventArgs : EventArgs {
        public bool isInvulnerable;
    }
    public class OnAttackActionEventArgs: EventArgs {
        public PickableSO weapon;
        public int weaponCount;
    }

    private void Awake() {
        Instance = this;

        health = entitySO.health;
        maxHealth = entitySO.health;
        isInvulnerable = false;
        rigidBody = GetComponent<Rigidbody2D>();
        FacingDirection = Vector2.right;

        foreach (PickableSO pickableWeapon in pickableWeapons.pickableSOList) {
            attackCountDictionary.Add(pickableWeapon, 0);
            if (currentAttackAction == null) {
                currentAttackAction = pickableWeapon;
            }
        }
    }

    private void Start() {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
        playerAnimator.OnFinishActing += PlayerAnimator_OnFinishActing;
        playerAnimator.OnDeath += PlayerAnimator_OnDeath;
    }

    private void FixedUpdate() {
        if (!GameManager.Instance.IsPlayable()) return;

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
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
        gameInput.OnAttackAction -= GameInput_OnAttackAction;
        playerAnimator.OnFinishActing -= PlayerAnimator_OnFinishActing;
        playerAnimator.OnDeath -= PlayerAnimator_OnDeath;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Handle Item Pickup
        other.transform.TryGetComponent<PickableObject>(out PickableObject pickedObject);
        if (pickedObject != null) {
            PickupItem(pickedObject);
        }
    }

    private void GameManager_OnGameOver(object sender, EventArgs e) {
        IsWalking = false;
        canAct = false;
        isInvulnerable = true;
    }

    private void GameInput_OnAttackAction(object sender, EventArgs e) {
        if (canAct && attackCountDictionary[currentAttackAction] > 0) {
            canAct = false;
            attackCountDictionary[currentAttackAction]--;
            OnAttackAction?.Invoke(this, new OnAttackActionEventArgs {
                weapon = currentAttackAction,
                weaponCount = attackCountDictionary[currentAttackAction]
            });
        }
    }

    private void PlayerAnimator_OnFinishActing(object sender, EventArgs e) {
        canAct = true;
    }

    private void PlayerAnimator_OnDeath(object sender, EventArgs e) {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage(float damage, Vector2 knockback) {
        if (!isInvulnerable) {
            health -= damage;
            rigidBody.AddForce(knockback, ForceMode2D.Impulse);
            OnHealthChange?.Invoke(this, new IDamageable.OnHealthChangeEventArgs{ healthNormalized = health / maxHealth });

            if (health <= 0) {
                canAct = false;
                isInvulnerable = true;
                OnDeath?.Invoke(this, EventArgs.Empty);
            } else {
                OnDamaged?.Invoke(this, EventArgs.Empty);
                StartCoroutine(InvulnerableStateCo());
                StartCoroutine(TemporaryActionDisableCo());
            }
        }
    }

    private IEnumerator InvulnerableStateCo() {
        if (invulnerableTimer > 0) {
            isInvulnerable = true;
            OnInvulnerableToggle?.Invoke(this, new OnInvulnerableToggleEventArgs{ isInvulnerable = true });
            yield return new WaitForSeconds(invulnerableTimer);
            isInvulnerable = false;
            OnInvulnerableToggle?.Invoke(this, new OnInvulnerableToggleEventArgs{ isInvulnerable = false });
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

    public PickableListSO GetPickableWeapons() {
        return pickableWeapons;
    }

    public void PickupItem(PickableObject pickedObject) {
        // Check if picked object is a weapon
        foreach (PickableSO pickableWeapon in pickableWeapons.pickableSOList) {
            if (pickedObject.GetPickableSO() == pickableWeapon) {
                attackCountDictionary[pickableWeapon]++;
                OnWeaponPickup?.Invoke(this, new OnWeaponPickupEventArgs{
                    pickedWeapon = pickableWeapon,
                    weaponCount = attackCountDictionary[pickableWeapon]
                });
                Destroy(pickedObject.gameObject);
                return;
            }
        }

        foreach (PickableSO pickableHealingItem in pickableHealingItems.pickableSOList) {
            if (pickedObject.GetPickableSO() == pickableHealingItem) {
                if (health < maxHealth) {
                    health = Mathf.Min(health + 1, maxHealth);
                    OnHealthChange?.Invoke(this, new IDamageable.OnHealthChangeEventArgs { healthNormalized = health / maxHealth });
                    OnHealthPickup?.Invoke(this, EventArgs.Empty);
                    Destroy(pickedObject.gameObject);
                    return;
                } else {
                    return;
                }
            }
        }
    }
}
