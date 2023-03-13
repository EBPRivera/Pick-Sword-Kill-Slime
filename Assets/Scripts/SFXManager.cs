using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {
    
    public static SFXManager Instance;

    [SerializeField] private SFXClipsSO sfxClipsSO;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Player.Instance.OnDamaged += Player_OnDamaged;
        Player.Instance.OnAttackAction += Player_OnAttackAction;
        Player.Instance.OnWeaponPickup += Player_OnWeaponPickup;
        Player.Instance.OnHealthPickup += Player_OnHealthPickup;
        Enemy.OnAnyDamaged += Enemy_OnAnyDamaged;
        Enemy.OnAnyDeath += Enemy_OnAnyDeath;
    }

    private void OnDestroy() {
        Player.Instance.OnDamaged -= Player_OnDamaged;
        Player.Instance.OnAttackAction -= Player_OnAttackAction;
        Player.Instance.OnWeaponPickup -= Player_OnWeaponPickup;
        Player.Instance.OnHealthPickup -= Player_OnHealthPickup;
    }

    private void Player_OnDamaged(object sender, EventArgs e) {
        PlaySound(sfxClipsSO.playerHurt, Player.Instance.transform.position);
    }

    private void Player_OnAttackAction(object sender, Player.OnAttackActionEventArgs e) {
        PlaySound(sfxClipsSO.swordSwings, Player.Instance.transform.position);
    }

    private void Player_OnWeaponPickup(object sender, Player.OnWeaponPickupEventArgs e) {
        PlaySound(sfxClipsSO.weaponPickup, Player.Instance.transform.position);
    }

    private void Player_OnHealthPickup(object sender, EventArgs e) {
        PlaySound(sfxClipsSO.healthPickup, Player.Instance.transform.position);
    }

    private void Enemy_OnAnyDamaged(object sender, EventArgs e) {
        Enemy enemy = sender as Enemy;
        PlaySound(sfxClipsSO.enemyHurt, enemy.transform.position);
    }

    private void Enemy_OnAnyDeath(object sender, EventArgs e) {
        Enemy enemy = sender as Enemy;
        PlaySound(sfxClipsSO.enemyDeath, enemy.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }
}
