using System;
using UnityEngine;

public interface IDamageable {
    public void TakeDamage(float damage, Vector2 knockback);
    public bool IsAlive();

    public event EventHandler OnDamaged;
    public event EventHandler OnDeath;
    public event EventHandler<OnHealthChangeEventArgs> OnHealthChange;
    public class OnHealthChangeEventArgs : EventArgs {
        public float healthNormalized;
    }
}