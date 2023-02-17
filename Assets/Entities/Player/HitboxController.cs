using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    private const string ENEMY = "Enemy";

    [SerializeField] private Player player;
    [SerializeField] private float swordDamage = 2f;
    [SerializeField] private float knockbackForce = 50f;

    private BoxCollider2D boxCollider;
    private Vector2 defaultHorizontalHitPos = new Vector2(0.35f, 0.11f);
    private Vector2 defaultVerticalHitPos = new Vector2(0, -0.015f);
    private Vector2 hitSize = new Vector2(0.42f, 0.48f);

    // Start is called before the first frame update
    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        player.OnTriggerAction += Player_OnTriggerAction;
    }

    private void OnDestroy() {
        player.OnTriggerAction -= Player_OnTriggerAction;
    }

    private void Player_OnTriggerAction(object sender, Player.OnTriggerActionEventArgs e) {
        if (e.direction.x != 0) {
            boxCollider.offset = e.direction.x > 0 ? defaultHorizontalHitPos : new Vector2(-defaultHorizontalHitPos.x, defaultHorizontalHitPos.y);
            boxCollider.size = hitSize;
        } else {
            boxCollider.offset = e.direction.y < 0 ? defaultVerticalHitPos : new Vector2(defaultVerticalHitPos.x, defaultVerticalHitPos.y + 0.391f);
            boxCollider.size = new Vector2(hitSize.y, hitSize.x);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == ENEMY) {
            IDamageable damageableObject = other.GetComponent<IDamageable>();

            if (damageableObject != null) {
                Vector2 enemyPosition = other.transform.position;
                Vector2 playerPosition = transform.parent.position;

                Vector2 direction = (Vector2) (enemyPosition - playerPosition).normalized;

                Vector2 knockback = direction * knockbackForce;

                damageableObject.TakeDamage(swordDamage, knockback);
            } else {
                Debug.LogWarning("Other object does not implement IDamageable interface");
            }
        }
    }
}
