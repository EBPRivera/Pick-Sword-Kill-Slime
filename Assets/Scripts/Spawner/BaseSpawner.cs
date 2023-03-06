using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour {
    [SerializeField] protected float width = 1;
    [SerializeField] protected float height = 1;
    [SerializeField] private int countMax = 5;
    [SerializeField] private float spawnTimerMax = 4f;

    [Header("Gizmos")]
    [SerializeField] private Color gizmoColor;
    
    private float spawnTimer = 0;

    private void Update() {
        if (GameManager.Instance.IsPlayable() && (transform.childCount < countMax || countMax <= 0)) {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > spawnTimerMax) {
                if (Spawn()) {
                    spawnTimer = 0f;
                }
            }
        }
    }

    protected virtual bool Spawn() {
        Debug.LogError("Spawner.Spawn()");
        return true;
    }

    private void OnDrawGizmos() {
        // Draw radius of bounds
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
    }
}
