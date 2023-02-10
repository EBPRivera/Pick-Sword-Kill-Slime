using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private float health;
    [SerializeField] private bool IsInvuln { get => _isInvuln;
        set {
            _isInvuln = value;

            if (_isInvuln && invulnTimer > 0) {
                StartCoroutine(TriggerInvulnCo());
            }
        }
    }
    public bool _isInvuln;
    [SerializeField] private float invulnTimer = 1;

    Animator animator;
    Rigidbody2D rb;

    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage, Vector2 knockback)
    {
        if (!IsInvuln) {
            IsInvuln = true;
            health -= damage;
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsInvuln) {
            IsInvuln = true;
            health -= damage;
        }
    }

    public void SetNotInvuln()
    {
        IsInvuln = false;
    }

    public bool IsInvulnerable() {
        return IsInvuln;
    }

    public float GetInvulnTimer() {
        return invulnTimer;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator TriggerInvulnCo() {
        yield return new WaitForSeconds(invulnTimer);
        IsInvuln = false;
    }
}