using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour {

    [SerializeField] PickableSO pickableSO;

    public static PickableObject SpawnPickableObject(PickableSO pickableSO, Vector2 position, Transform parent = null) {
        Transform pickableObjectTransform = Instantiate(pickableSO.prefab, parent);

        PickableObject pickableObject = pickableObjectTransform.GetComponent<PickableObject>();
        Vector2 colliderSize = pickableObject.GetComponent<Collider2D>().bounds.size;
        Collider2D overlap = Physics2D.OverlapBox(position, colliderSize + colliderSize * 0.1f, 0);

        if (overlap == null) {
            pickableObjectTransform.position = position;
            return pickableObject;
        } else {
            Destroy(pickableObjectTransform.gameObject);
            return null;
        }
    }

    public PickableSO GetPickableSO() {
        return pickableSO;
    }
}
