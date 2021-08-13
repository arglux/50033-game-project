using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public float spreadAngle;
    public int numShots;

    protected override void fire(Vector2 aimVector)
    {
        Vector2 position = spawnDistance * aimVector;
        for (int i = 0; i < this.numShots; i++)
        {
            float angle = Mathf.Lerp(-this.spreadAngle, this.spreadAngle, (float)i / (this.numShots - 1));
            this.spawnBullet(position, Weapon.Rotate(aimVector, angle));
            
        }
    }
}