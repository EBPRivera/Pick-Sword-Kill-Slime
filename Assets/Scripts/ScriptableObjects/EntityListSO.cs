using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EntityListSO")]
public class EntityListSO : ScriptableObject {
    public List<EntitySO> entitySOList;
}
