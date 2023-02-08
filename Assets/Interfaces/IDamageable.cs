using UnityEngine;

public interface IDamageable {
    public float Health { set; get; }
    public bool IsInvuln { set; get; }
    void TakeDamage(float damage, Vector2 knockback);
    void TakeDamage(float damage);
    void SetNotInvuln();
    void Die();
}