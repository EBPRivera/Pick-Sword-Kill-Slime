using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IDamageable
{
    public float Health {
        set {
            _health = value;

            if (_health <= 0) {
                animator.SetTrigger("Death");
            } else {
                animator.SetTrigger("Damaged");
            }
        }
        get {
            return _health;
        }
    }
    public float _health = 3f;
    public bool IsInvuln {
        set {
            _isInvuln = value;
        }
        get {
            return _isInvuln;
        }
    }
    public bool _isInvuln = false;
    public float knockbackForce = 500f;
    public float damage = 1f;

    Animator animator;
    Rigidbody2D rb;

    void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            IDamageable damageableObject = other.gameObject.GetComponent<IDamageable>();

            if (damageableObject != null) {
                damageableObject.TakeDamage(damage, (Vector2) (other.transform.position - gameObject.transform.position).normalized * knockbackForce);
            }
        }    
    }

    public void SetNotInvuln() {
        IsInvuln = false;
    }

    public void TakeDamage(float damage, Vector2 knockback)
    {
        Health -= damage;
        IsInvuln = true;

        rb.AddForce(knockback);
    }

    public void TakeDamage(float damage) {
        Health -= damage;
        IsInvuln = true;
    }

    public void Die() {
        Destroy(gameObject);
    }
}
