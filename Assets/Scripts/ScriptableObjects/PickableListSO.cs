using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PickableListSO")]
public class PickableListSO : ScriptableObject {
    public List<PickableSO> pickableSOList;
}
