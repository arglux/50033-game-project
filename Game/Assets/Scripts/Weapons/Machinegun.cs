using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machinegun : Weapon
{
    protected override void fire(Vector2 aimVector)
    {
        this.spawnBullet(0.9f * aimVector, aimVector);
        // AudioManager.SharedInstance.Play(AudioManager.SharedInstance.machinegunSound);
    }

    protected override AudioSource playReload()
    {
        return null; // return AudioManager.SharedInstance.Play(AudioManager.SharedInstance.machinegunReloadSound, false);
    }
}