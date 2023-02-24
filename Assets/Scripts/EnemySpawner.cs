using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private EntityListSO entityListSO;
    [SerializeField] private float spawnRadius;

    [Header("Gizmos")]
    [SerializeField] private Color gizmoColor;
    
    private int enemyCountMax = 5;
    private float enemySpawnTimer;
    private float enemySpawnTimerMax = 4f;

    private void Start() {
        enemySpawnTimer = 0;
    }

    private void Update() {
        if (GameManager.Instance.IsPlayable()) {
            if (transform.childCount < enemyCountMax) {
                enemySpawnTimer += Time.deltaTime;

                if (enemySpawnTimer > enemySpawnTimerMax) {
                    EntitySO enemyToSpawn = entityListSO.entitySOList[UnityEngine.Random.Range(0, entityListSO.entitySOList.Count)];
                    SpawnEnemy(enemyToSpawn);
                }
            }
        }
    }

    private void SpawnEnemy(EntitySO enemyToSpawn) {
        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        float distance = UnityEngine.Random.Range(0, spawnRadius);
        Vector2 spawnPoint = (Vector2)transform.position + direction * distance;

        if (Enemy.SpawnEnemy(enemyToSpawn, spawnPoint, transform) != null) {
            enemySpawnTimer = 0f;
        }
    }

    private void OnDrawGizmos() {
        // Draw radius of bounds
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
