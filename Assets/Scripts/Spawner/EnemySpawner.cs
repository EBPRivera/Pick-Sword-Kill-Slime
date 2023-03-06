using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : BaseSpawner {

    [Header("Spawn List")]
    [SerializeField] private EntityListSO entityListSO;

    protected override bool Spawn() {
        EntitySO enemyToSpawn = entityListSO.entitySOList[UnityEngine.Random.Range(0, entityListSO.entitySOList.Count)];
        Vector2 spawnPoint = new Vector2(
            transform.position.x + UnityEngine.Random.Range(0, width) - width / 2,
            transform.position.y + UnityEngine.Random.Range(0, height) - height / 2
        );

        return Enemy.SpawnEnemy(enemyToSpawn, spawnPoint, transform) != null;
    }
}
