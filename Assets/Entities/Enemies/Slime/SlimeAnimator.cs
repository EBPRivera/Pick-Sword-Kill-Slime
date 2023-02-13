using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    private string DAMAGED = "Damaged";
    private string DEATH = "Death";

    private SpriteRenderer sprite;
    private Animator animator;
    private HealthController healthController;
    private SlimeController slimeController;
    private bool isBlinking;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthController = GetComponentInParent<HealthController>();
        slimeController = GetComponentInParent<SlimeController>();
    }

    private void Start() {
        healthController.OnInvulnerableTrigger += HealthController_OnInvulnerableTrigger;
        healthController.OnVulnerableTrigger += HealthController_OnVulnerableTrigger;
        healthController.OnDamage += HealthController_OnDamage;
    }

    private void FixedUpdate() {
        if (slimeController.FacingDirection.x < 0) {
            sprite.flipX = true;
        } else {
            sprite.flipX = false;
        }
    }

    private void OnDestroy() {
        healthController.OnInvulnerableTrigger -= HealthController_OnInvulnerableTrigger;
        healthController.OnVulnerableTrigger -= HealthController_OnVulnerableTrigger;
        healthController.OnDamage -= HealthController_OnDamage;
    }

    private void HealthController_OnInvulnerableTrigger(object sender, EventArgs e) {
        isBlinking = true;
        StartCoroutine(BlinkingCo());
    }

    private void HealthController_OnVulnerableTrigger(object sender, EventArgs e) {
        isBlinking = false;
    }

    private void HealthController_OnDamage(object sender, EventArgs e) {
        if (healthController.Health <= 0) {
            animator.SetTrigger(DEATH);
        }

        animator.SetTrigger(DAMAGED);
    }

    private IEnumerator BlinkingCo() {
        while (isBlinking) {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.2f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
