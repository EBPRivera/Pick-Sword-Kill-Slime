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
        targetCollider.gameObject.TryGetComponent<Player>(out Player player);

        if (targetCollider != null && player != null && player.IsAlive()) {
            float distanceFromTarget = Vector2.Distance(transform.position, targetCollider.transform.position);
            Vector2 directionToTarget = (Vector2) (targetCollider.transform.position - transform.position).normalized;

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, directionToTarget, distanceFromTarget);

            if (raycastHit.transform == targetCollider.transform) {
                SetDetected(targetCollider.transform);
            } else {
                SetDetected();
            }
        } else {
            SetDetected();
        }
    }

    private void SetDetected(Transform target = null) {
        DetectedEntity = target;
        Detected = target != null;
    }
}
