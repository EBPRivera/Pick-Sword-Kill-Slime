using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health;
    [SerializeField] private float invulnTimer = 1;

    public float Health { get => _health; private set => _health = value; }
    public bool IsInvuln { get => _isInvuln;
        private set {
            _isInvuln = value;

            if (_isInvuln && invulnTimer > 0) {
                StartCoroutine(TriggerInvulnCo());
            }
        }
    }
    private bool _isInvuln;
    private Rigidbody2D rb;

    public event EventHandler OnInvulnerableTrigger;
    public event EventHandler OnVulnerableTrigger;
    public event EventHandler OnDamage;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage, Vector2 knockback)
    {
        if (!IsInvuln) {
            Health -= damage;
            IsInvuln = true;
            OnDamage?.Invoke(this, EventArgs.Empty);
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsInvuln) {
            Health -= damage;
            IsInvuln = true;
            OnDamage?.Invoke(this, EventArgs.Empty);
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
        OnInvulnerableTrigger?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(invulnTimer);
        IsInvuln = false;
        OnVulnerableTrigger?.Invoke(this, EventArgs.Empty);
    }
}