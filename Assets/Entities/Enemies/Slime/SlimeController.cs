using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float knockbackForce = 50f;
    public float damage = 1f;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            IDamageable damageableObject = other.gameObject.GetComponent<IDamageable>();

            if (damageableObject != null) {
                damageableObject.TakeDamage(damage, (Vector2) (other.transform.position - gameObject.transform.position).normalized * knockbackForce);
            }
        }    
    }
}
