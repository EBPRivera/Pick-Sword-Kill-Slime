using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EntitySO")]
public class EntitySO : ScriptableObject {
    public Transform prefab;
    public float health;
    public float damage;
    public float knockbackForce;
    public float movementSpeed;
    public string entityName;
}
