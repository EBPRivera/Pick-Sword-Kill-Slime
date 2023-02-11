using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string HORIZONTAL_DIRECTION = "HorizontalDirection";
    private const string VERTICAL_DIRECTION = "VerticalDirection";

    private PlayerController playerController;
    private HealthController healthController;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool isBlinking = false;

    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
        healthController = GetComponentInParent<HealthController>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        healthController.OnInvulnerableTrigger += HealthController_OnInvulnerableTrigger;
        healthController.OnVulnerableTrigger += HealthController_OnVulnerableTrigger;
    }

    private void FixedUpdate() {
        Vector2 facingDirection = playerController.FacingDirection;
        
        animator.SetBool(IS_WALKING, playerController.IsWalking);
        animator.SetFloat(HORIZONTAL_DIRECTION, facingDirection.x);
        animator.SetFloat(VERTICAL_DIRECTION, facingDirection.y);

        if (facingDirection.x < 0) {
            sprite.flipX = true;
        } else {
            sprite.flipX = false;
        }
    }

    private void OnDestroy() {
        healthController.OnInvulnerableTrigger -= HealthController_OnInvulnerableTrigger;
        healthController.OnVulnerableTrigger -= HealthController_OnVulnerableTrigger;
    }

    private void HealthController_OnInvulnerableTrigger(object sender, EventArgs e) {
        if (healthController.Health <= 0) return;

        isBlinking = true;
        StartCoroutine(BlinkingCo());
    }

    private void HealthController_OnVulnerableTrigger(object sender, EventArgs e) {
        isBlinking = false;
    }

    private IEnumerator BlinkingCo() {
        while (isBlinking) {
            sprite.enabled = true;
            yield return new WaitForSeconds(0.2f);
            sprite.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }
        sprite.enabled = true;
    }

    public void TriggerAction(string action) {
        animator.SetTrigger(action);
    }
}
