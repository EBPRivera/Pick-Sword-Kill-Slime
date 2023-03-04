using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCountSingleUI : MonoBehaviour {
    
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI weaponCountText;
    private PickableSO pickableSO;
    private int weaponCount = 0;

    public PickableSO GetPickableSO() {
        return pickableSO;
    }

    public void SetPickableSO(PickableSO pickableSO) {
        this.pickableSO = pickableSO;

        icon.sprite = pickableSO.sprite;
    }

    public void SetWeaponCount(int weaponCount) {
        this.weaponCount = weaponCount;

        weaponCountText.text = "x" + weaponCount.ToString();

        gameObject.SetActive(weaponCount > 0);
    }
}
