using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayUI : MonoBehaviour {

    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private Button backButton;

    private void Awake() {
        backButton.onClick.AddListener(() => {
            Hide();
            mainMenuUI.Show();
        });
    }

    private void Start() {
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
