using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    fromPlayer = 0,
    fromEnemy = 1
}

public class BulletController : MonoBehaviour
{
    public float despawnTime = 5.0f;

    public BulletType bulletType;
    public DamageTypes damageType;

    public Vector3 position;
    public Vector3 direction;

    public Sprite defaultSprite;

    public float moveSpeed;
    public float baseDamage;
    public int pierceCount;

    public float healthMultiplier = 1.0f;
    public float shieldMultiplier = 1.0f;
    public float armourMultiplier = 1.0f;

    void OnEnable()
    {
        this.gameObject.transform.position = this.position;
        this.gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, this.direction);
        StartCoroutine(DespawnRoutine());
    }

    void Update()
    {

        this.gameObject.transform.Translate(Vector3.up * this.moveSpeed * Time.deltaTime);
    }

    void Despawn()
    {
        this.transform.Find("sprite").gameObject.GetComponent<SpriteRenderer>().sprite = this.defaultSprite;
        this.gameObject.SetActive(false);
    }

    IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(this.despawnTime);
        this.Despawn();
    }

    public float GetMultiplierFor(HealthType type)
    {
        switch (type)
        {
            case HealthType.health:
                return healthMultiplier;
            case HealthType.shield:
                return shieldMultiplier;
            case HealthType.armour:
                return armourMultiplier;
            default:
                return 0.5f;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.gameObject;
        if (target.CompareTag("Enemy"))
        {
            if (bulletType == BulletType.fromPlayer)
            {
                this.pierceCount--;
                target.GetComponent<EnemyHealthController>().TakeDamage(this);
                if (this.pierceCount <= 0) this.Despawn();
            }
        }
        else if (target.CompareTag("Wall"))
        {
            this.Despawn();
        }
        else if (target.CompareTag("Player"))
        {
            if (bulletType == BulletType.fromEnemy)
            {
                this.pierceCount--;
                target.GetComponent<PlayerController>().TakeDamage(this);
                if (this.pierceCount <= 0) this.Despawn();
            }
        }
    }
}
