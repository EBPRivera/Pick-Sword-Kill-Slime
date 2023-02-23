using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {
    private const string DEATH = "Death";
    private const string DAMAGED = "Damaged";

    [SerializeField] private EntitySO entitySO;
    [SerializeField] private Detector detector;
    [SerializeField] private EnemyAnimator enemyAnimator;

    public bool IsMoving { get; private set; }
    public Vector2 FacingDirection { get; private set; }
    private float health;
    private bool isInvulnerable;
    private bool canMove = true;
    private Rigidbody2D rigidBody;

    public event EventHandler<OnHitEventArgs> OnHit;
    public class OnHitEventArgs : EventArgs {
        public string trigger;
    }

    private void Awake() {
        health = entitySO.health;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        enemyAnimator.OnVulnerableTrigger += EnemyAnimator_OnVulnerabletrigger;
        enemyAnimator.OnDeath += EnemyAnimator_OnDeath;
    }

    private void FixedUpdate() {
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
        enemyAnimator.OnVulnerableTrigger -= EnemyAnimator_OnVulnerabletrigger;
        enemyAnimator.OnDeath -= EnemyAnimator_OnDeath;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (health > 0) {
            other.transform.TryGetComponent<Player>(out Player player);
            player?.TakeDamage(entitySO.damage, (Vector2) (other.transform.position - transform.position).normalized * entitySO.knockbackForce);
        }
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
            isInvulnerable = true;
            rigidBody.AddForce(knockback, ForceMode2D.Impulse);

            if (health <= 0) {
                canMove = false;
                OnHit?.Invoke(this, new OnHitEventArgs {trigger = DEATH});
            } else {
                StartCoroutine(TemporaryDisableMovement());
                OnHit?.Invoke(this, new OnHitEventArgs {trigger = DAMAGED});
            }
        }
    }

    private IEnumerator TemporaryDisableMovement() {
        canMove = false;
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

}
