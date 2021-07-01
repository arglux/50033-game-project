using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    protected override void fire(Vector2 aimVector)
    {
        this.spawnBullet(1.1f * aimVector, aimVector);
        AudioManager.SharedInstance.Play(AudioManager.SharedInstance.rifleSound);
    }

    protected override AudioSource playReload()
    {
        return AudioManager.SharedInstance.Play(AudioManager.SharedInstance.rifleReloadSound, false);
    }
}