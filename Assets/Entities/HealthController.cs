using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    public float Health { get => _health;
        set {
            _health = value;

            if (_health < 0) {
                animator.SetTrigger("Death");
            } else {
                animator.SetTrigger("Damaged");
            }
        }
    }
    public bool IsInvuln { get => _isInvuln;
        set {
            _isInvuln = value;

            if (_isInvuln && invulnTimer > 0) {
                StartCoroutine(TriggerInvulnCo());
            }
        }
    }
    public float _health;
    public bool _isInvuln;
    public float invulnTimer = 1;

    Animator animator;
    Rigidbody2D rb;

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage, Vector2 knockback)
    {
        if (!IsInvuln) {
            IsInvuln = true;
            Health -= damage;
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsInvuln) {
            IsInvuln = true;
            Health -= damage;
        }
    }

    public void SetNotInvuln()
    {
        IsInvuln = false;
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