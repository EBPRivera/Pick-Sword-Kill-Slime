using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private LayerMask detectionLayerMask;
    [Header("Parent")]
    [SerializeField] private Transform entity;

    private bool detected;

    public event EventHandler<OnPlayerDetectedEventArgs> OnPlayerDetected;
    public class OnPlayerDetectedEventArgs : EventArgs {
        public Transform followTarget;
    }

    private void Awake() {
        detected = false;
    }

    private void FixedUpdate() {
        Collider2D targetCollider = Physics2D.OverlapCircle(entity.position, detectionRadius, detectionLayerMask);

        if (targetCollider != null) {
            targetCollider.transform.TryGetComponent<Player>(out Player player);

            if (!HasObstacle(targetCollider.transform) && player != null && player.IsAlive()) {
                SetDetected(targetCollider.transform);
            } else {
                SetDetected();
            }
        } else {
            SetDetected();
        }
    }

    private bool HasObstacle(Transform target) {
        float distanceFromTarget = Vector2.Distance(transform.position, target.position);
        Vector2 directionToTarget = (Vector2) (target.position - transform.position).normalized;

        int collisionCount = Physics2D.Raycast(
            transform.position,
            directionToTarget,
            new ContactFilter2D { useTriggers = false },
            new List<RaycastHit2D>(),
            distanceFromTarget
        );

        return collisionCount > 1;
    }

    private void SetDetected(Transform target = null) {
        if (!detected && target != null || detected && target == null) {
            detected = !detected;
            OnPlayerDetected?.Invoke(this, new OnPlayerDetectedEventArgs {
                followTarget = target
            });
        }
    }
}
