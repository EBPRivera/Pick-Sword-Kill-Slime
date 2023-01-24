using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObjectController : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    
    float distanceToCover = 0;
    float speed = 3;
    Vector2 moveDirection = new Vector2(0, 0);
    Rigidbody2D rigidBody;
    List<RaycastHit2D> collidedObjects = new List<RaycastHit2D>();

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (distanceToCover > 0) {
            int collision = rigidBody.Cast(
                moveDirection, contactFilter, collidedObjects, speed * Time.fixedDeltaTime
            );

            if (collision == 0) {
                rigidBody.MovePosition(rigidBody.position + moveDirection * speed * Time.fixedDeltaTime);
                distanceToCover -= speed * Time.fixedDeltaTime;
            }
        }
    }

    public void MoveTo(Vector2 direction) {
        // TODO: Clean up code
        if (direction.x != 0) {
            moveDirection = new Vector2(direction.x > 0 ? 1 : -1, 0);
            distanceToCover = 1;
        } else if (direction.y != 0) {
            moveDirection = new Vector2(0, direction.y > 0 ? 1 : -1);
            distanceToCover = 1;
        }
    }
}
