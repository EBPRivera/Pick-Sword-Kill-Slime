using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjectSpawner : MonoBehaviour {
    
    [SerializeField] private PickableListSO pickableListSO;
    [SerializeField] private float width;
    [SerializeField] private float height;

    [Header("Gizmo")]
    [SerializeField] private Color gizmoColor;

    private float pickableObjectSpawnTimer = 0f;
    private float pickableObjectSpawnTimerMax = 3f;
    private int pickableObjectCountMax = 4;

    private void Update() {
        if (GameManager.Instance.IsPlayable() && transform.childCount <= pickableObjectCountMax) {
            pickableObjectSpawnTimer += Time.deltaTime;

            if (pickableObjectSpawnTimer > pickableObjectSpawnTimerMax) {
                // Add an object within the bounded box
                PickableSO pickableObjectToSpawn = pickableListSO.pickableSOList[UnityEngine.Random.Range(0, pickableListSO.pickableSOList.Count)];
                SpawnPickableObject(pickableObjectToSpawn);
            }
        }
    }

    private void SpawnPickableObject(PickableSO pickableObjectToSpawn) {
        Vector2 position = new Vector2(
            UnityEngine.Random.Range(0, width) - width / 2,
            UnityEngine.Random.Range(0, height) - height / 2
        );

        if (PickableObject.SpawnPickableObject(pickableObjectToSpawn, position, transform) != null) {
            pickableObjectSpawnTimer = 0f;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
    }
}
