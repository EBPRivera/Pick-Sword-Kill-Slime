using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    public PlayerController playerController;
    BoxCollider2D boxCollider;
    Vector2 defaultHorizontalPosition = new Vector2(0.09f, -0.13f);
    Vector2 defaultVerticalPosition = new Vector2(0, -0.21f);

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushDirection(Vector2 direction) {
        // Get the direction of player facing
        // Prioritize side facing
        if (direction.x != 0) {
            boxCollider.offset = direction.x > 0 ? defaultHorizontalPosition : new Vector2(-defaultHorizontalPosition.x, defaultHorizontalPosition.y);
        } else {
            boxCollider.offset = direction.y < 0 ? defaultVerticalPosition : new Vector2(defaultVerticalPosition.x, defaultVerticalPosition.y + 0.16f);
        }

        boxCollider.enabled = true;
    }

    public void StopPushing() {
        boxCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PushableObject") {
            print("Detected pushable object");
        }
    }
}
