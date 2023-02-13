using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    private const string ATTACK = "Attack";

    [SerializeField] private float swordDamage = 2f;
    [SerializeField] private float knockbackForce = 50f;

    private BoxCollider2D boxCollider;
    private Vector2 defaultHorizontalHitPos = new Vector2(0.35f, 0.11f);
    private Vector2 defaultVerticalHitPos = new Vector2(0, -0.015f);
    private Vector2 hitSize = new Vector2(0.42f, 0.48f);
    private string action = "";

    // Start is called before the first frame update
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void SetAction(string act, Vector2 direction) {
        if (act == ATTACK) {
            if (direction.x != 0) {
                boxCollider.offset = direction.x > 0 ? defaultHorizontalHitPos : new Vector2(-defaultHorizontalHitPos.x, defaultHorizontalHitPos.y);
                boxCollider.size = hitSize;
            } else {
                boxCollider.offset = direction.y < 0 ? defaultVerticalHitPos : new Vector2(defaultVerticalHitPos.x, defaultVerticalHitPos.y + 0.391f);
                boxCollider.size = new Vector2(hitSize.y, hitSize.x);
            }
        }
        action = act;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (action == "Attack" && other.tag == "Enemy") {
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
