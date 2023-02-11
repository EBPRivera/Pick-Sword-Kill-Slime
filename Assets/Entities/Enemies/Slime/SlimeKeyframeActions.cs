using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKeyframeActions : MonoBehaviour
{
    private HealthController healthController;

    private void Awake() {
        healthController = GetComponentInParent<HealthController>();
    }

    public void SetNotInvuln() {
        healthController.SetNotInvuln();
    }

    public void Die() {
        Destroy(healthController.gameObject);
    }
}
