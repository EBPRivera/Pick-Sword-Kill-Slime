using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string HORIZONTAL_DIRECTION = "HorizontalDirection";
    private const string VERTICAL_DIRECTION = "VerticalDirection";
    private const string ATTACK = "Attack";
    private const string PUSH = "Push";
    private const string DEATH = "Death";

    [SerializeField] private Player player;

    private Animator animator;
    private SpriteRenderer sprite;
    private bool isBlinking = false;
    private Vector3 leftFacingLocalScale = new Vector3(-1, 1, 1);

    public event EventHandler OnFinishActing;
    public event EventHandler OnDeath;

    private void Awake() {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        player.OnInvulnerableToggle += Player_OnInvulnerableToggle;
        player.OnAttackAction += Player_OnAttackAction;
        player.OnPushAction += Player_OnPushAction;
        player.OnDeath += Player_OnDeath;
    }

    private void FixedUpdate() {
        animator.SetBool(IS_WALKING, player.IsWalking);
        animator.SetFloat(HORIZONTAL_DIRECTION, player.FacingDirection.x);
        animator.SetFloat(VERTICAL_DIRECTION, player.FacingDirection.y);

        HandleHorizontalFlip();
    }

    private void HandleHorizontalFlip() {
        if (transform.localScale.x < 0 && player.FacingDirection.x >= 0) {
            // Face right when player is facing left
            transform.localScale = Vector3.one;
        } else if (transform.localScale.x > 0 && player.FacingDirection.x < 0) {
            // Face left when player is facing right
            transform.localScale = leftFacingLocalScale;
        }
    }

    private void OnDestroy() {
        player.OnInvulnerableToggle -= Player_OnInvulnerableToggle;
        player.OnAttackAction -= Player_OnAttackAction;
        player.OnPushAction -= Player_OnPushAction;
        player.OnDeath -= Player_OnDeath;
    }

    private void Player_OnInvulnerableToggle(object sender, Player.OnInvulnerableToggleEventArgs e) {
        if (e.isInvulnerable) {
            if (e.health <= 0) return;

            isBlinking = true;
            StartCoroutine(BlinkingCo());
        } else {
            isBlinking = false;
        }
    }

    private void Player_OnPushAction(object sender, EventArgs e) {
        animator.SetTrigger(PUSH);
    }

    private void Player_OnAttackAction(object sender, EventArgs e) {
        animator.SetTrigger(ATTACK);
    }

    private void Player_OnDeath(object sender, EventArgs e) {
        animator.SetTrigger(DEATH);
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

    private void OnFinishActingKeyframeEvent() {
        OnFinishActing?.Invoke(this, EventArgs.Empty);
    }

    private void OnDeathKeyframeEvent() {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }
}
