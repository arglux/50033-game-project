using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public float health = 100;
    public float moveSpeed;

    void OnEnable()
    {
        this.health = 100;
    }

    void Update()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, this.player.transform.position, Time.deltaTime * this.moveSpeed);
    }

    public void Impact(BulletController bullet)
    {
        AudioManager.SharedInstance.Play(AudioManager.SharedInstance.ouchSound);
        this.health -= bullet.baseDamage;
        if (this.health <= 0)
        {
            this.Die();
        }
    }

    void Die()
    {
        AudioManager.SharedInstance.Play(AudioManager.SharedInstance.dieSound);
        this.gameObject.SetActive(false);
    }
}
