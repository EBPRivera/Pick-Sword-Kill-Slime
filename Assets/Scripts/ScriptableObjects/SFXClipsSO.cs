using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio/SFXClips")]
public class SFXClipsSO : ScriptableObject {

    public AudioClip[] playerHurt;
    public AudioClip[] swordSwings;
    public AudioClip[] enemyHurt;
    public AudioClip[] enemyDeath;
    public AudioClip[] weaponPickup;
    public AudioClip[] healthPickup;
    
}
