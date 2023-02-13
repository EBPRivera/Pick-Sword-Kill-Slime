using UnityEngine;

public interface IDamageable {
    void TakeDamage(float damage, Vector2 knockback);
    void TakeDamage(float damage);
    void SetNotInvuln();
    void Die();
}