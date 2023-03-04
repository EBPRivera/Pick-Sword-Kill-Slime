using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PickableSO")]
public class PickableSO : ScriptableObject {
    public Transform prefab;
    public Sprite sprite;
    public string pickableObjectName;
}
