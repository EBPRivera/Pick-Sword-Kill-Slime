using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    private const string PLAYER = "Player";

    [SerializeField] private EntitySO entitySO;
    
    [Header("Children")]
    [SerializeField] private Detector detector;

    private bool canMove = true;
    // private Detector detector;
    private Rigidbody2D rigidBody;
    private HealthController healthController;

    public Vector2 FacingDirection { get; private set; }

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        healthController = GetComponent<HealthController>();
    }

    private void Start() {
        healthController.OnDamage += HealthController_OnDamage;
    }

    private void OnDestroy() {
        healthController.OnDamage -= HealthController_OnDamage;
    }

    private void FixedUpdate() {
        if (detector.Detected && canMove) {
            Vector2 direction = (Vector2) (detector.DetectedEntity.transform.position - gameObject.transform.position).normalized;
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity + (direction * entitySO.movementSpeed * Time.fixedDeltaTime), entitySO.maxSpeed);
            FacingDirection = direction;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == PLAYER && healthController.Health > 0) {
            IDamageable damageableObject = other.gameObject.GetComponent<IDamageable>();

            if (damageableObject != null) {
                damageableObject.TakeDamage(entitySO.damage, (Vector2) (other.transform.position - gameObject.transform.position).normalized * entitySO.knockbackForce);
            }
        }
    }

    private void HealthController_OnDamage(object sender, EventArgs e) {
        if (healthController.Health <= 0) {
            canMove = false;
        } else {
            StartCoroutine(TemporaryDisableMovement());
        }
    }

    private IEnumerator TemporaryDisableMovement() {
        canMove = false;
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }
}
