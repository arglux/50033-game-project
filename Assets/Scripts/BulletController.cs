using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float despawnTime = 3.0f;
    
    public Vector3 position;
    public Vector3 direction;

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
        this.gameObject.SetActive(false);
    }

    IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(this.despawnTime);
        this.Despawn();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject target = col.gameObject;
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyController>().Impact(this);
            this.pierceCount--;
            if (this.pierceCount <= 0) this.Despawn();
        }
    }
}
