using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour {

    [SerializeField] PickableSO pickableSO;

    private Collider2D objectCollider;

    private void Awake() {
        objectCollider = GetComponent<Collider2D>();
    }

    private Vector2 GetColliderSize() {
        return objectCollider.bounds.size;
    }

    public PickableSO GetPickableSO() {
        return pickableSO;
    }

    public static PickableObject SpawnPickableObject(PickableSO pickableSO, Vector2 position, Transform parent = null) {
        Transform pickableObjectTransform = Instantiate(pickableSO.prefab, parent);

        PickableObject pickableObject = pickableObjectTransform.GetComponent<PickableObject>();
        Vector2 colliderSize = pickableObject.GetColliderSize();

        int overlapCount = Physics2D.OverlapBox(
            position,
            colliderSize + colliderSize * 0.1f,
            0,
            new ContactFilter2D { useTriggers = true },
            new List<Collider2D>()
        );

        if (overlapCount == 0) {
            pickableObjectTransform.position = position;
            return pickableObject;
        } else {
            Destroy(pickableObjectTransform.gameObject);
            return null;
        }
    }
}
