using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private const string IS_MOVING = "IsMoving";

    [SerializeField] private Enemy enemy;
    private SpriteRenderer sprite;
    private Animator animator;

    public event EventHandler OnDeath;
    public event EventHandler OnVulnerableTrigger;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        enemy.OnHit += Enemy_OnHit;
    }


    private void FixedUpdate() {
        sprite.flipX = enemy.FacingDirection.x < 0;
        animator.SetBool(IS_MOVING, enemy.IsMoving);
    }

    private void OnDestroy() {
        enemy.OnHit -= Enemy_OnHit;
    }

    private void Enemy_OnHit(object sender, Enemy.OnHitEventArgs e) {
        animator.SetTrigger(e.trigger);

    }

    private void OnDeathKeyframeEvent() {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    private void OnVulnerableTriggerKeyframeEvent() {
        OnVulnerableTrigger?.Invoke(this, EventArgs.Empty);
    }
}
