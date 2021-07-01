using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float spawnInterval;
    public float spawnDistance;
    public GameObject player;

    public static EnemyManager SharedInstance;

    void Start()
    {
        EnemyManager.SharedInstance = this;
        StartCoroutine(SpawnEnemyRoutine());
    }

    public void SpawnEnemy()
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(ObjectType.enemy);
        EnemyController enemy = item.GetComponent<EnemyController>();
        enemy.player = this.player;
        item.transform.position = this.player.transform.position + (Vector3)(spawnDistance * Random.insideUnitCircle.normalized);
        item.SetActive(true);
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.spawnInterval);
            this.SpawnEnemy();
        }
    }
}
