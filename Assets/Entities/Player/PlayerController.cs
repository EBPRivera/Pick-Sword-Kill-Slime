using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3;
    public float collisionOffset = 0.02f;
    public float health = 3f;
    public ContactFilter2D contactFilter;
    public HitboxController HitboxController;

    Vector2 inputDirection;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> collisionList = new List<RaycastHit2D>();
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
            MovementHandler();
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

    public void CleanupHitbox() {
        canAct = true;
        HitboxController.DisableCollider();
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

    private void MovementHandler() {
        bool moveCheck = TryMove(inputDirection);

        if (!moveCheck) {
            moveCheck = TryMove(new Vector2(inputDirection.x, 0));
        }
        if (!moveCheck) {
            moveCheck = TryMove(new Vector2(0, inputDirection.y));
        }

        facingDirection = inputDirection;
        animator.SetBool("isWalking", moveCheck);
    }

    private bool TryMove(Vector2 input) {
        if (input == Vector2.zero) return false;

        int collisionCount = rigidBody.Cast(
            input, contactFilter, collisionList, movementSpeed * Time.fixedDeltaTime + collisionOffset
        );

        if (collisionCount == 0) {
            rigidBody.MovePosition(rigidBody.position + input * movementSpeed * Time.fixedDeltaTime);
            return true;
        }

        return false;
    }
}
