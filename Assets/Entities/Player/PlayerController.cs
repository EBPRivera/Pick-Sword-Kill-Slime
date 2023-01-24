using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3;
    public float collisionOffset = 0.02f;
    public ContactFilter2D contactFilter;
    public PushController pushController;

    Vector2 inputDirection;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> collisionList = new List<RaycastHit2D>();
    Vector2 facingDirection = new Vector2(1, 0);
    bool canMove = true;

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
        if (inputDirection != Vector2.zero && canMove) {
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
        canMove = false;
        pushController.PushDirection(facingDirection);
        animator.SetTrigger("Push");
    }

    public void FinishPush() {
        canMove = true;
        pushController.StopPushing();
    }

    public Vector2 GetFacingDirection() {
        return facingDirection;
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
