using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private const string IS_MOVING = "IsMoving";
    private const string DEATH = "Death";
    private const string DAMAGED = "Damaged";

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
        enemy.OnDeath += Enemy_OnDeath;
        enemy.OnDamaged += Enemy_OnDamaged;
    }

    private void FixedUpdate() {
        sprite.flipX = enemy.FacingDirection.x < 0;
        animator.SetBool(IS_MOVING, enemy.IsMoving);
    }

    private void OnDestroy() {
        enemy.OnDeath -= Enemy_OnDeath;
        enemy.OnDamaged -= Enemy_OnDamaged;
    }

    private void Enemy_OnDamaged(object sender, EventArgs e) {
        animator.SetTrigger(DAMAGED);
    }

    private void Enemy_OnDeath(object sender, EventArgs e) {
        animator.SetTrigger(DEATH);
    }

    private void OnDeathKeyframeEvent() {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    private void OnVulnerableTriggerKeyframeEvent() {
        OnVulnerableTrigger?.Invoke(this, EventArgs.Empty);
    }
}
