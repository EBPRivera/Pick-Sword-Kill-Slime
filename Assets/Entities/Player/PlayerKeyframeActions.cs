using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyframeActions : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void FinishActing() {
        playerController.FinishActing();
    }

    public void Die() {
        // Trigger game over screen
    }
}
