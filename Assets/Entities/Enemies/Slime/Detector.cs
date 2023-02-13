using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private const string PLAYER = "Player";

    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private GameObject target;

    public bool Detected { get; private set; }
    public GameObject DetectedEntity { get; private set; }

    private GameObject entity;

    private void Awake() {
        entity = gameObject.transform.parent.gameObject;
    }

    private void FixedUpdate() {
        float distanceFromTarget = Vector3.Distance(entity.transform.position, target.transform.position);
        Vector2 directionToTarget = (Vector2) (target.transform.position - entity.transform.position).normalized;

        if (distanceFromTarget <= detectionRadius) {
            RaycastHit2D raycastHit = Physics2D.Raycast(entity.transform.position, directionToTarget, distanceFromTarget);
            if (raycastHit.transform.CompareTag(PLAYER)) {
                Detected = true;
                DetectedEntity = target;
            } else {
                Detected = false;
            }
        } else {
            Detected = false;
        }
    }
}
