using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObjectController : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    
    float speed = 3;
    bool canMove = true;
    Vector2 moveDirection = new Vector2(0, 0);
    Vector3 destination;
    Rigidbody2D rigidBody;
    List<RaycastHit2D> collidedObjects = new List<RaycastHit2D>();

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        destination = transform.position;
    }

    void FixedUpdate() {
        if (transform.position != destination) {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
        } else {
            canMove = true;
        }
    }

    public void MoveTo(Vector2 direction) {
        if (!canMove) return;

        if (direction.x != 0) {
            SetDestination(new Vector2(direction.x, 0));
        } else if (direction.y != 0) {
            SetDestination(new Vector2(0, direction.y));
        }
    }

    private void SetDestination(Vector2 direction) {
        int collisionCount = rigidBody.Cast(
            direction, contactFilter, collidedObjects, 1
        );

        if (collisionCount == 0) {
            destination = transform.position + new Vector3(direction.x, direction.y).normalized;
            canMove = false;
        }
    }
}
