using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public PlayerController playerController;
    BoxCollider2D boxCollider;
    Vector2 defaultHorizontalPushPos = new Vector2(0.2565f, 0.1f);
    Vector2 defaultVerticalPushPos = new Vector2(0, -0.1065f);
    Vector2 defaultHorizontalHitPos = new Vector2(0.35f, 0.11f);
    Vector2 defaultVerticalHitPos = new Vector2(0, -0.015f);
    Vector2 pushSize = new Vector2(0.2f, 0.2f);
    Vector2 hitSize = new Vector2(0.42f, 0.48f);
    private string action = "";

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void SetAction(string act, Vector2 direction) {
        // Offset and Size handling
        switch (act) {
            case "Push":
                if (direction.x != 0) {
                    boxCollider.offset = direction.x > 0 ? defaultHorizontalPushPos : new Vector2(-defaultHorizontalPushPos.x, defaultHorizontalPushPos.y);
                } else {
                    boxCollider.offset = direction.y < 0 ? defaultVerticalPushPos : new Vector2(defaultVerticalPushPos.x, defaultVerticalPushPos.y + 0.419f);
                }
                boxCollider.size = pushSize;
                break;
            case "Attack":
                if (direction.x != 0) {
                    boxCollider.offset = direction.x > 0 ? defaultHorizontalHitPos : new Vector2(-defaultHorizontalHitPos.x, defaultHorizontalHitPos.y);
                    boxCollider.size = hitSize;
                } else {
                    boxCollider.offset = direction.y < 0 ? defaultVerticalHitPos : new Vector2(defaultVerticalHitPos.x, defaultVerticalHitPos.y + 0.391f);
                    boxCollider.size = new Vector2(hitSize.y, hitSize.x);
                }
                break;
        }
        action = act;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (action == "Push" && other.tag == "PushableObject") {
            // Get player's direction
            Vector2 facingDirection = playerController.GetFacingDirection();

            // Move object based on player's direction
            other.gameObject.GetComponent<PushableObjectController>().MoveTo(facingDirection);
        } else if (action == "Attack" && other.tag == "Enemy") {
            other.gameObject.SendMessage("TakeDamage", 2f);
        }
    }
}
