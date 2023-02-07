using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 150f;
    public float maxSpeed = 8f;
    public float health = 3f;
    public HitboxController HitboxController;

    Vector2 inputDirection;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 facingDirection = new Vector2(1, 0);
    bool canAct = true;
    bool isInvuln = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Update player's movement
        if (inputDirection != Vector2.zero && canAct) {
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity + (inputDirection * movementSpeed * Time.fixedDeltaTime), maxSpeed);
            facingDirection = inputDirection;
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }

        animator.SetFloat("HorizontalDirection", facingDirection.x);
        animator.SetFloat("VerticalDirection", facingDirection.y);

        // side facing
        if (facingDirection.x < 0 && Mathf.Abs(facingDirection.x) >= Mathf.Abs(facingDirection.y)) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }

    void OnMove(InputValue input) {
        inputDirection = input.Get<Vector2>();
    }

    void OnPush() {
        ActivateHitBox("Push");
    }

    public void OnAttack() {
        ActivateHitBox("Attack");
    }

    private void ActivateHitBox(string trigger) {
        if (canAct) {
            canAct = false;
            HitboxController.SetAction(trigger, facingDirection);
            animator.SetTrigger(trigger);
        }
    }

    public void FinishActing() {
        canAct = true;
    }

    public Vector2 GetFacingDirection() {
        return facingDirection;
    }

    public void TakeDamage(float damage) {
        if (!isInvuln) {
            health -= damage;
            StartCoroutine(TriggerInvulnStateCo());
        }
    }

    private IEnumerator TriggerInvulnStateCo() {
        isInvuln = true;
        float endTime = Time.time + 2f;

        while (Time.time < endTime) {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        isInvuln = false;
    }
}
