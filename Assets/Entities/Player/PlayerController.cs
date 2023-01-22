using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1;
    public ContactFilter2D contactFilter;

    Vector2 inputDirection;
    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> collisionList = new List<RaycastHit2D>();
    float horizontalDirection = 0;
    float verticalDirection = 0;

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
        if (inputDirection != Vector2.zero) {
            // Check player collision at given direction
            int collisionCount = rigidBody.Cast(
                inputDirection, contactFilter, collisionList, movementSpeed * Time.fixedDeltaTime
            );

            // Move player if no collisions detected at the given direction
            if (collisionCount == 0) {
                rigidBody.MovePosition(rigidBody.position + inputDirection * movementSpeed * Time.fixedDeltaTime);

                // Change direction
                horizontalDirection = inputDirection.x;
                verticalDirection = inputDirection.y;
            }

            animator.SetFloat("HorizontalDirection", horizontalDirection);
            animator.SetFloat("VerticalDirection", verticalDirection);
            animator.SetBool("isWalking", true);

            // side facing
            if (horizontalDirection < 0 && Mathf.Abs(horizontalDirection) >= Mathf.Abs(verticalDirection)) {
                spriteRenderer.flipX = true;
            } else {
                spriteRenderer.flipX = false;
            }
        } else {
            animator.SetBool("isWalking", false);
        }
    }

    void OnMove(InputValue input) {
        inputDirection = input.Get<Vector2>();
    }
}
