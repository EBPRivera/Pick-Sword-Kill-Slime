using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjectSpawner : MonoBehaviour {
    
    [SerializeField] private PickableListSO pickableListSO;
    [SerializeField] private float width = 1;
    [SerializeField] private float height = 1;
    [SerializeField] private int objectCountMax = 4;
    [SerializeField] private float spawnTimerMax = 3f;

    [Header("Gizmo")]
    [SerializeField] private Color gizmoColor;

    private float spawnTimer = 0f;

    private void Update() {
        if (GameManager.Instance.IsPlayable() && transform.childCount <= objectCountMax) {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > spawnTimerMax) {
                PickableSO pickableObjectToSpawn = pickableListSO.pickableSOList[UnityEngine.Random.Range(0, pickableListSO.pickableSOList.Count)];
                SpawnPickableObject(pickableObjectToSpawn);
            }
        }
    }

    private void SpawnPickableObject(PickableSO pickableObjectToSpawn) {
        Vector2 position = new Vector2(
            transform.position.x + UnityEngine.Random.Range(0, width) - width / 2,
            transform.position.y + UnityEngine.Random.Range(0, height) - height / 2
        );

        if (PickableObject.SpawnPickableObject(pickableObjectToSpawn, position, transform) != null) {
            spawnTimer = 0f;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
    }
}
