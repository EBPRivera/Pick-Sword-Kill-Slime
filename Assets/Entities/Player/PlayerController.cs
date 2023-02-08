using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 150f;
    public float maxSpeed = 8f;
    public HitboxController HitboxController;

    bool canAct = true;
    Vector2 inputDirection;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 FacingDirection {
        set {
            _facingDirection = value;

            animator.SetFloat("HorizontalDirection", _facingDirection.x);
            animator.SetFloat("VerticalDirection", _facingDirection.y);
        }
        get {
            return _facingDirection;
        }
    }
    Vector2 _facingDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        FacingDirection = new Vector2(1, 0);
    }

    void FixedUpdate()
    {
        // Update player's movement
        if (inputDirection != Vector2.zero && canAct) {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity + (inputDirection * movementSpeed * Time.fixedDeltaTime), maxSpeed);
            FacingDirection = inputDirection;
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }

        // side facing
        if (FacingDirection.x < 0 && Mathf.Abs(FacingDirection.x) >= Mathf.Abs(FacingDirection.y)) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }

    void OnMove(InputValue input) {
        inputDirection = input.Get<Vector2>();
    }

    void OnPush() {
        ActivateHitBox("Push");
    }

    public void OnAttack() {
        ActivateHitBox("Attack");
    }

    private void ActivateHitBox(string trigger) {
        if (canAct) {
            canAct = false;
            HitboxController.SetAction(trigger, FacingDirection);
            animator.SetTrigger(trigger);
        }
    }

    public void FinishActing() {
        canAct = true;
    }

    public Vector2 GetFacingDirection() {
        return FacingDirection;
    }

    // public void StartInvulnCoroutine() {
    //     StartCoroutine(TriggerInvulnStateCo());
    // }

    // private IEnumerator TriggerInvulnStateCo() {
    //     IsInvuln = true;
    //     canAct = false;
    //     float endTime = Time.time + 2f;

    //     while (Time.time < endTime) {
    //         spriteRenderer.enabled = false;
    //         yield return new WaitForSeconds(0.2f);
    //         spriteRenderer.enabled = true;
    //         yield return new WaitForSeconds(0.2f);
    //         canAct = true;
    //     }

    //     IsInvuln = false;
    // }
}
