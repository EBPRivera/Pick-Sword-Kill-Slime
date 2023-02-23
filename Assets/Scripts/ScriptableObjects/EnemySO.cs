using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject {
    public Transform prefab;
    public float health;
    public float damage;
    public float knockbackForce;
    public float movementSpeed;
    public string enemyName;
}
