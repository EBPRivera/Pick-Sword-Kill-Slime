using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private LayerMask detectionLayerMask;
    [Header("Parent")]
    [SerializeField] private Transform entity;

    public bool Detected { get; private set; }
    public Transform DetectedEntity { get; private set; }

    private void Awake() {
        Detected = false;
    }

    private void FixedUpdate() {
        Collider2D targetCollider = Physics2D.OverlapCircle(entity.position, detectionRadius, detectionLayerMask);

        if (targetCollider != null) {
            targetCollider.transform.TryGetComponent<Player>(out Player player);

            if (!IsObstacle(targetCollider.transform) && player != null && player.IsAlive()) {
                SetDetected(targetCollider.transform);
            } else {
                SetDetected();
            }
        } else {
            SetDetected();
        }
    }

    private bool IsObstacle(Transform target) {
        float distanceFromTarget = Vector2.Distance(transform.position, target.position);
        Vector2 directionToTarget = (Vector2) (target.position - transform.position).normalized;

        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, directionToTarget, distanceFromTarget);

        return raycastHit.transform != target;
    }

    private void SetDetected(Transform target = null) {
        DetectedEntity = target;
        Detected = target != null;
    }
}
