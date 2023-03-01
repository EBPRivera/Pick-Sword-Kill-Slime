using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private const string ENEMY = "Enemy";

    [SerializeField] private Player player;

    private BoxCollider2D boxCollider;
    private Vector2 defaultHorizontalHitPosition;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        defaultHorizontalHitPosition = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == ENEMY) {
            IDamageable damageableObject = other.GetComponent<IDamageable>();

            if (damageableObject != null) {
                Vector2 enemyPosition = other.transform.position;
                Vector2 playerPosition = transform.parent.position;

                Vector2 direction = (Vector2) (enemyPosition - playerPosition).normalized;

                Vector2 knockback = direction * player.GetEntitySO().knockbackForce;

                damageableObject.TakeDamage(player.GetEntitySO().damage, knockback);
            } else {
                Debug.LogWarning("Other object does not implement IDamageable interface");
            }
        }
    }
}
