using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour {

    [SerializeField] PickableSO pickableSO;

    private void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.TryGetComponent<Player>(out Player player);
        if (player != null) {
            player.ItemPickup(pickableSO);
            Destroy(gameObject);
        }
    }
}
