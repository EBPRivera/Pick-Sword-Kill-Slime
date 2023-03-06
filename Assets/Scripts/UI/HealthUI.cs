using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {
    [SerializeField] private Transform entity;
    [SerializeField] private Image healthBar;

    private IDamageable hasHealth;

    private void Awake() {
        hasHealth = entity.GetComponent<IDamageable>();

        if (hasHealth == null) {
            Debug.LogError("Entity does not implement IDamageable");
            Destroy(gameObject);
        }
    }

    private void Start() {
        hasHealth.OnHealthChange += HasHealth_OnHealthChange;

        gameObject.SetActive(false);
    }

    private void HasHealth_OnHealthChange(object sender, IDamageable.OnHealthChangeEventArgs e) {
        if (e.healthNormalized < 1) {
            gameObject.SetActive(true);
            healthBar.fillAmount = e.healthNormalized;
        } else {
            gameObject.SetActive(false);
        }
    }
}
