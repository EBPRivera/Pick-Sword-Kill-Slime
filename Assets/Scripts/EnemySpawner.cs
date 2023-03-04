using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private EntityListSO entityListSO;
    [SerializeField] private float width = 1;
    [SerializeField] private float height = 1;
    [SerializeField] private int enemyCountMax = 5;
    [SerializeField] private float spawnTimerMax = 4f;

    [Header("Gizmos")]
    [SerializeField] private Color gizmoColor;
    
    private float spawnTimer = 0;

    private void Update() {
        if (GameManager.Instance.IsPlayable() && transform.childCount < enemyCountMax) {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > spawnTimerMax) {
                EntitySO enemyToSpawn = entityListSO.entitySOList[UnityEngine.Random.Range(0, entityListSO.entitySOList.Count)];
                SpawnEnemy(enemyToSpawn);
            }
        }
    }

    private void SpawnEnemy(EntitySO enemyToSpawn) {
        Vector2 spawnPoint = new Vector2(
            transform.position.x + UnityEngine.Random.Range(0, width) - width / 2,
            transform.position.y + UnityEngine.Random.Range(0, height) - height / 2
        );

        if (Enemy.SpawnEnemy(enemyToSpawn, spawnPoint, transform) != null) {
            spawnTimer = 0f;
        }
    }

    private void OnDrawGizmos() {
        // Draw radius of bounds
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
    }
}
