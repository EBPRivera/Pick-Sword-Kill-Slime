using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private GameObject target;

    private GameObject entity;

    private void Awake() {
        entity = gameObject.transform.parent.gameObject;
    }

    private void FixedUpdate() {
        float distanceFromTarget = Vector3.Distance(entity.transform.position, target.transform.position);
        Vector2 directionToTarget = (Vector2) (target.transform.position - entity.transform.position);

        if (distanceFromTarget <= detectionRadius) {
            RaycastHit2D raycastHit = Physics2D.Raycast(entity.transform.position, directionToTarget, distanceFromTarget);
            if (raycastHit.transform.CompareTag("Player")) {
                Debug.Log("Player Detected");
            }
        }
    }
}
