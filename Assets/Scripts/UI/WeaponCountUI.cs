using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCountUI : MonoBehaviour {
    
    [SerializeField] Player player;
    [SerializeField] Transform weaponIconTemplate;

    private void Awake() {
        InstantiateWeaponCountUI();
        weaponIconTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        player.OnWeaponPickup += Player_OnWeaponPickup;
        player.OnAttackAction += Player_OnAttackAction;
    }

    private void OnDestroy() {
        player.OnWeaponPickup -= Player_OnWeaponPickup;
        player.OnAttackAction -= Player_OnAttackAction;
    }

    private void Player_OnAttackAction(object sender, Player.OnAttackActionEventArgs e) {
        SetWeaponCountUI(e.weapon, e.weaponCount);
    }

    private void Player_OnWeaponPickup(object sender, Player.OnWeaponPickupEventArgs e) {
        SetWeaponCountUI(e.pickedWeapon, e.weaponCount);
    }

    private void InstantiateWeaponCountUI() {
        foreach (PickableSO pickableSO in player.GetPickableWeapons().pickableSOList) {
            Transform weaponIcon = Instantiate(weaponIconTemplate, transform);
            weaponIcon.GetComponent<WeaponCountSingleUI>().SetPickableSO(pickableSO);
            weaponIcon.GetComponent<WeaponCountSingleUI>().SetWeaponCount(0);
        }
    }

    private void SetWeaponCountUI(PickableSO weapon, int count) {
        // Update values of passed weapon
        foreach (Transform child in transform) {
            if (child == weaponIconTemplate) continue;

            if (child.GetComponent<WeaponCountSingleUI>().GetPickableSO() == weapon) {
                child.GetComponent<WeaponCountSingleUI>().SetWeaponCount(count);
                break;
            }
        }
    }
}
