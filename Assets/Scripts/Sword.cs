using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    private int swordCount = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.TryGetComponent<Player>(out Player player);
        if (player != null) {
            // Add sword count to player
            player.SetSwordCount(player.GetSwordCount() + swordCount);
            Destroy(gameObject);
        }
    }
}
