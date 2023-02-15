using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/DamageableSo")]
public class DamageableSO : ScriptableObject {
    public float health;
    public float invulnerableTimer;
    public string objectName;
}
