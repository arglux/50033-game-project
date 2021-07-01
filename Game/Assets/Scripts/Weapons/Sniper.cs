using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    protected override void fire(Vector2 aimVector)
    {
        this.spawnBullet(1.1f * aimVector, aimVector);
        // AudioManager.SharedInstance.Play(AudioManager.SharedInstance.sniperSound);
    }

    protected override AudioSource playReload()
    {
        return null; // return AudioManager.SharedInstance.Play(AudioManager.SharedInstance.sniperReloadSound, false);
    }
}