using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyType spawnType;
    public float Timer = 2f;
    public Vector3 spawnPoint = new Vector3(0, 0, 0);
    public bool isSpawning;
    public int maxSpawn = 2;
    public int spawnCount = 0;

    public List<EnemyPoolItem> enemiesToPool; // types of different object to pool 
    public List<ExistingPoolEnemy> pooledEnemies; // a list of all objects in the pool, of all types

    void Awake()
    {
        pooledEnemies = new List<ExistingPoolEnemy>();

        foreach (EnemyPoolItem enemy in enemiesToPool)
        {
            for (int i = 0; i < enemy.amount; i++)
            {
                // this 'instance' a local variable, but Unity will not remove it since it exists in the scene
                GameObject instance = (GameObject) Instantiate(enemy.prefab);
                instance.GetComponent<StateController>().SetWaypointList(enemy.waypoints);
                instance.GetComponent<StateController>().SetLastSeen(enemy.investigationPoint);
                instance.SetActive(false);
                instance.transform.parent = this.transform;

                ExistingPoolEnemy e = new ExistingPoolEnemy(instance, enemy.type);
                pooledEnemies.Add(e);
            }
        }
    }

    // TODO: Fix naming conventions, spin support classes out?
    void Start()
    {
        isSpawning = true;
        // for (int j = 0; j < 1; j++)
        //     spawnEnemyFromPooler(EnemyType.walker);
    }

    // Update is called once per frame
    void Update()
    {
        // if (ChildrenAllInactive()) isSpawning = true;
        // if (ChildrenAllActive()) isSpawning = false;
        // if (isSpawning)
        // Debug.Log(isSpawning);
        if (isSpawning) SpawnEveryFixedSecond();
        if (spawnCount > maxSpawn) isSpawning = false;
    }

    void SpawnEveryFixedSecond()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0) 
        {
            spawnEnemyFromPooler(spawnType);
            Timer = 2f;
        }
    }
    public void isDead() {
        isSpawning = false;
    }

    void spawnEnemyFromPooler(EnemyType type)
    {
        GameObject enemy = GetPooledEnemies(type);
        // Debug.Log(enemy);

        if (enemy != null)
        {
            //set position
            // enemy.transform.localScale = new Vector3(1, 1, 1);
            enemy.transform.position = spawnPoint;
            enemy.SetActive(true);
            spawnCount++;
        }
        else
        {
            // Debug.Log("not enough enemies in the pool!");
        }
    }

    private GameObject GetPooledEnemies(EnemyType type)
    {
        // return inactive pooled object if it matches the type 
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (!pooledEnemies[i].gameObject.activeInHierarchy && pooledEnemies[i].type == type)
            {
                return pooledEnemies[i].gameObject;
            }
        }
        // this will be called when no more active object is present, item to expand pool if required 
        foreach (EnemyPoolItem enemy in enemiesToPool)
        {
            if (enemy.type == type)
            {
                if (enemy.expandPool)
                {
                    GameObject instance = (GameObject) Instantiate (enemy.prefab);
                    instance.SetActive(false);
                    instance.transform.parent = this.transform;
                    pooledEnemies.Add(new ExistingPoolEnemy(instance, enemy.type));
                    return instance;
                }
            }
        }

        // we will return null IF and only IF the type doesn't match with what is defined in the enemiesToPool. 
        return null;
    }


    private bool ChildrenAllInactive()
    {
        foreach (ExistingPoolEnemy enemy in pooledEnemies) 
        {
            if (enemy.gameObject.activeInHierarchy) {
                return false;
            }
        }
        return true;
    }

    private bool ChildrenAllActive()
    {
        foreach (ExistingPoolEnemy enemy in pooledEnemies) 
        {
            if (!enemy.gameObject.activeInHierarchy) {
                return false;
            }
        }
        return true;
    }
}
