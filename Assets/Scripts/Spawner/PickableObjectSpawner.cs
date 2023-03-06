using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjectSpawner : BaseSpawner {
    
    [Header("Spawn List")]
    [SerializeField] private PickableListSO pickableListSO;

    protected override bool Spawn() {
        PickableSO objectToSpawn = pickableListSO.pickableSOList[UnityEngine.Random.Range(0, pickableListSO.pickableSOList.Count)];
        Vector2 position = new Vector2(
            transform.position.x + UnityEngine.Random.Range(0, width) - width / 2,
            transform.position.x + UnityEngine.Random.Range(0, height) - height / 2
        );

        return PickableObject.SpawnPickableObject(objectToSpawn, position, transform) != null;
    }
}
