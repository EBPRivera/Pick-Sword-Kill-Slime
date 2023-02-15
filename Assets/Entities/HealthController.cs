using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private DamageableSO damageableSO;

    private float invulnerableTimer;
    private Rigidbody2D rb;

    public float Health { get; private set; }
    public bool IsInvuln { get; private set; }

    public event EventHandler OnInvulnerableTrigger;
    public event EventHandler OnVulnerableTrigger;
    public event EventHandler OnDamage;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Health = damageableSO.health;
        invulnerableTimer = damageableSO.invulnerableTimer;
    }

    public void TakeDamage(float damage, Vector2 knockback)
    {
        if (!IsInvuln) {
            Health -= damage;
            IsInvuln = true;
            if (invulnerableTimer > 0) {
                StartCoroutine(TriggerInvulnCo());
            }
            OnDamage?.Invoke(this, EventArgs.Empty);
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsInvuln) {
            Health -= damage;
            IsInvuln = true;
            if (invulnerableTimer > 0) {
                StartCoroutine(TriggerInvulnCo());
            }
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
        yield return new WaitForSeconds(invulnerableTimer);
        IsInvuln = false;
        OnVulnerableTrigger?.Invoke(this, EventArgs.Empty);
    }
}