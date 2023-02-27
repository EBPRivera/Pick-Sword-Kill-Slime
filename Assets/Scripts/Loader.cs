using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader{
    public enum Scene {
        MainMenuScene,
        GameScene
    }

    public static void LoadScene(Scene targetScene) {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
