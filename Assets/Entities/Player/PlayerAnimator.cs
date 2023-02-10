using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string HORIZONTAL_DIRECTION = "HorizontalDirection";
    private const string VERTICAL_DIRECTION = "VerticalDirection";

    private PlayerController playerController;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        Vector2 facingDirection = playerController.GetFacingDirection();
        
        animator.SetBool(IS_WALKING, playerController.IsWalking());
        animator.SetFloat(HORIZONTAL_DIRECTION, facingDirection.x);
        animator.SetFloat(VERTICAL_DIRECTION, facingDirection.y);

        if (facingDirection.x < 0) {
            sprite.flipX = true;
        } else {
            sprite.flipX = false;
        }
    }

    public void TriggerAction(string action) {
        animator.SetTrigger(action);
    }

    public void TriggerBlink(bool status) {
        sprite.enabled = status;
    }

    public void FinishActing() {
        playerController.FinishActing();
    }
}
