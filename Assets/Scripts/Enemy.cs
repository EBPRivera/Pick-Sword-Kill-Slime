using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {

    public static event EventHandler OnAnyDeath;
    public static event EventHandler OnAnyDamaged;

    public static void ResetStaticData() {
        OnAnyDeath = null;
        OnAnyDamaged = null;
    }
    
    [SerializeField] private EntitySO entitySO;
    [SerializeField] private Detector detector;
    [SerializeField] private EnemyAnimator enemyAnimator;

    public bool IsMoving { get; private set; }
    public Vector2 FacingDirection { get; private set; }
    private float health;
    private float maxHealth;
    private bool isInvulnerable;
    private bool canMove = true;
    private Rigidbody2D rigidBody;
    private Collider2D enemyCollider;

    public event EventHandler OnDamaged;
    public event EventHandler OnDeath;
    public event EventHandler<IDamageable.OnHealthChangeEventArgs> OnHealthChange;

    private void Awake() {
        health = entitySO.health;
        maxHealth = entitySO.health;
        rigidBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Start() {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        enemyAnimator.OnVulnerableTrigger += EnemyAnimator_OnVulnerabletrigger;
        enemyAnimator.OnDeath += EnemyAnimator_OnDeath;
    }

    private void FixedUpdate() {
        if (!GameManager.Instance.IsPlayable()) return;

        if (detector.Detected && canMove) {
            Vector2 direction = (Vector2) (detector.DetectedEntity.position - transform.position).normalized;
            rigidBody.velocity = rigidBody.velocity + (direction * entitySO.movementSpeed * Time.fixedDeltaTime);
            IsMoving = true;
            FacingDirection = direction;
        } else {
            IsMoving = false;
        }
    }

    private void OnDestroy() {
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
        enemyAnimator.OnVulnerableTrigger -= EnemyAnimator_OnVulnerabletrigger;
        enemyAnimator.OnDeath -= EnemyAnimator_OnDeath;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (health > 0) {
            other.transform.TryGetComponent<Player>(out Player player);
            player?.TakeDamage(entitySO.damage, (Vector2) (other.transform.position - transform.position).normalized * entitySO.knockbackForce);
        }
    }

    private void GameManager_OnGameOver(object sender, EventArgs e) {
        IsMoving = false;
        canMove = false;
    }

    private void EnemyAnimator_OnVulnerabletrigger(object sender, EventArgs e) {
        isInvulnerable = false;
    }

    private void EnemyAnimator_OnDeath(object sender, EventArgs e) {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, Vector2 knockback) {
        if (!isInvulnerable) {
            health -= damage;
            OnHealthChange?.Invoke(this, new IDamageable.OnHealthChangeEventArgs { healthNormalized = health / maxHealth });
            isInvulnerable = true;
            rigidBody.AddForce(knockback, ForceMode2D.Impulse);

            if (health <= 0) {
                canMove = false;
                OnDeath?.Invoke(this, EventArgs.Empty);
                OnAnyDeath?.Invoke(this, EventArgs.Empty);
            } else {
                StartCoroutine(TemporaryDisableMovement());
                OnDamaged?.Invoke(this, EventArgs.Empty);
                OnAnyDamaged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool IsAlive() {
        return health > 0;
    }

    private IEnumerator TemporaryDisableMovement() {
        canMove = false;
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

    private Vector2 GetColliderSize() {
        return enemyCollider.bounds.size;
    }

    public static Enemy SpawnEnemy(EntitySO enemySO, Vector2 position, Transform parent = null) {
        Transform enemyTransform = Instantiate(enemySO.prefab, parent);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        float colliderSizeMagnitude = enemy.GetColliderSize().magnitude;
        float spawnRadius = colliderSizeMagnitude + colliderSizeMagnitude * 0.01f;

        List<Collider2D> colliderList = new List<Collider2D>();
        int overlapCount = Physics2D.OverlapCircle(position, spawnRadius, new ContactFilter2D { useTriggers = false }, colliderList);

        if (overlapCount == 0) {
            enemyTransform.position = position;
            return enemy;
        } else {
            Destroy(enemyTransform.gameObject);
            return null;
        }
    }
}
