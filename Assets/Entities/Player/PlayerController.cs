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
    List<RaycastHit2D> collisionList = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Update player's movement
        // Check player collision at given direction
        int collisionCount = rigidBody.Cast(
            inputDirection, contactFilter, collisionList, movementSpeed * Time.fixedDeltaTime
        );

        // Move player if no collisions detected at the given direction
        if (collisionCount == 0) {
            rigidBody.MovePosition(rigidBody.position + inputDirection * movementSpeed * Time.fixedDeltaTime);
        }
    }

    void OnMove(InputValue input) {
        inputDirection = input.Get<Vector2>();
    }
}
