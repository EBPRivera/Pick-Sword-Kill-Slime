using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    private const string IS_MOVING = "IsMoving";

    [SerializeField] private Slime slime;
    private SpriteRenderer sprite;
    private Animator animator;

    public event EventHandler OnDeath;
    public event EventHandler OnVulnerableTrigger;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        slime.OnHit += Slime_OnHit;
    }


    private void FixedUpdate() {
        sprite.flipX = slime.FacingDirection.x < 0;
        animator.SetBool(IS_MOVING, slime.IsMoving);
    }

    private void OnDestroy() {
        slime.OnHit -= Slime_OnHit;
    }

    private void Slime_OnHit(object sender, Slime.OnHitEventArgs e) {
        animator.SetTrigger(e.trigger);

    }

    private void OnDeathKeyframeEvent() {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    private void OnVulnerableTriggerKeyframeEvent() {
        OnVulnerableTrigger?.Invoke(this, EventArgs.Empty);
    }
}
