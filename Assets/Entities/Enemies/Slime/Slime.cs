using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IDamageable
{
    private const string PLAYER = "Player";
    private const string DEATH = "Death";
    private const string DAMAGED = "Damaged";

    [SerializeField] private EntitySO entitySO;
    [SerializeField] private DamageableSO damageableSO;
    [SerializeField] private Detector detector;
    [SerializeField] private SlimeAnimator slimeAnimator;

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
        health = damageableSO.health;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        slimeAnimator.OnVulnerableTrigger += SlimeAnimator_OnVulnerabletrigger;
        slimeAnimator.OnDeath += SlimeAnimator_OnDeath;
    }

    private void FixedUpdate() {
        if (detector.Detected && canMove) {
            Vector2 direction = (Vector2) (detector.DetectedEntity.position - transform.position).normalized;
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity + (direction * entitySO.movementSpeed * Time.fixedDeltaTime), entitySO.maxSpeed);
            IsMoving = true;
            FacingDirection = direction;
        } else {
            IsMoving = false;
        }
    }

    private void OnDestroy() {
        slimeAnimator.OnVulnerableTrigger -= SlimeAnimator_OnVulnerabletrigger;
        slimeAnimator.OnDeath -= SlimeAnimator_OnDeath;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == PLAYER && health > 0) {
            IDamageable damageableObject = other.gameObject.GetComponent<IDamageable>();

            if (damageableObject != null) {
                damageableObject.TakeDamage(entitySO.damage, (Vector2) (other.transform.position - gameObject.transform.position).normalized * entitySO.knockbackForce);
            }
        }
    }

    private void SlimeAnimator_OnVulnerabletrigger(object sender, EventArgs e) {
        isInvulnerable = false;
    }

    private void SlimeAnimator_OnDeath(object sender, EventArgs e) {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, Vector2 knockback) {
        HandleDamagedBehaviour(damage, knockback);
    }

    public void TakeDamage(float damage) {
        HandleDamagedBehaviour(damage, Vector2.zero);
    }

    private void HandleDamagedBehaviour(float damage, Vector2 knockback) {
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
