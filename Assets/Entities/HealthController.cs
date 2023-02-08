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
        }
    }
    public float _health;
    public bool _isInvuln;

    Animator animator;
    Rigidbody2D rb;

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage, Vector2 knockback)
    {
        if (!IsInvuln) {
            Health -= damage;
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsInvuln) {
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
}