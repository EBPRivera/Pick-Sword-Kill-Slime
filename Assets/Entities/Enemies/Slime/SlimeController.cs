using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
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
    public bool isInvuln = false;

    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }    
    }

    public void TakeDamage(float damage) {
        Health -= damage;
        isInvuln = true;
    }

    public void SetNotInvuln() {
        isInvuln = false;
    }

    public void Die() {
        Destroy(gameObject);
    }
}
