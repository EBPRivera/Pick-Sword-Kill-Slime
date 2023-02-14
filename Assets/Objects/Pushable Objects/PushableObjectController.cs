using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObjectController : MonoBehaviour, IPushable
{
    private const float DEFAULT_DISTANCE = 1f;

    [SerializeField] private ContactFilter2D contactFilter;
    
    private float speed = 3;
    private bool canMove = true;
    private Vector2 moveDirection = new Vector2(0, 0);
    private Vector3 destination;
    private Rigidbody2D rigidBody;
    private List<RaycastHit2D> collidedObjects = new List<RaycastHit2D>();

    private void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        destination = transform.position;
    }

    private void FixedUpdate() {
        if (transform.position != destination) {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
        } else {
            canMove = true;
        }
    }

    private void SetDestination(Vector3 pusherPosition, float distance = DEFAULT_DISTANCE) {
        if (!canMove) return;

        Vector3 closestPoint = rigidBody.ClosestPoint(pusherPosition);
        Vector2 pushDirection = (closestPoint - pusherPosition).normalized;

        if (Mathf.Abs(pushDirection.y) > Mathf.Abs(pushDirection.x)) {
            pushDirection = new Vector2(0, pushDirection.y).normalized;
        } else {
            pushDirection = new Vector2(pushDirection.x, 0).normalized;
        }

        int collisionCount = rigidBody.Cast(pushDirection, contactFilter, collidedObjects, distance);

        if (collisionCount == 0) {
            destination = transform.position + (Vector3) pushDirection * distance;
            canMove = false;
        }
    }

    public void Push(Vector3 pusherPosition) {
        SetDestination(pusherPosition);
    }

    public void Push(Vector3 pusherPosition, float distance) {
        SetDestination(pusherPosition, distance);
    }
}
