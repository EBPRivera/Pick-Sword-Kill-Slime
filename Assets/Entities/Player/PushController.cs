using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    public PlayerController playerController;
    BoxCollider2D boxCollider;
    Vector2 defaultHorizontalPosition = new Vector2(0.2565f, 0.1f);
    Vector2 defaultVerticalPosition = new Vector2(0, -0.1065f);

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }

    public void PushDirection(Vector2 direction) {
        // Get the direction of player facing
        // Prioritize side facing
        if (direction.x != 0) {
            boxCollider.offset = direction.x > 0 ? defaultHorizontalPosition : new Vector2(-defaultHorizontalPosition.x, defaultHorizontalPosition.y);
        } else {
            boxCollider.offset = direction.y < 0 ? defaultVerticalPosition : new Vector2(defaultVerticalPosition.x, defaultVerticalPosition.y + 0.419f);
        }

        boxCollider.enabled = true;
    }

    public void StopPushing() {
        boxCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PushableObject") {
            // Trigger the object's move function based on the direction of the player
            // Get player's direction
            Vector2 facingDirection = playerController.GetFacingDirection();
            // Move object based on player's direction
            other.gameObject.GetComponent<PushableObjectController>().MoveTo(facingDirection);
        }
    }
}
