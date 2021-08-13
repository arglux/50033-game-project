using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    walker = 0,
    flyer = 1,
    shooter = 2,
    rusher = 3,
    jinn = 4,
    dragon = 5,
    medusa = 6,
    demon = 7,
    lizard = 8
}

[System.Serializable]
public class EnemyPoolItem
{
   public int amount;
   public GameObject prefab;
   public bool expandPool;
   public EnemyType type;
   public Transform investigationPoint;
   public List<Transform> waypoints;
   
}

public class ExistingPoolEnemy
{
    public GameObject gameObject;
    public EnemyType type;

    public ExistingPoolEnemy(GameObject gameObject, EnemyType type){
        // reference input 
        this.gameObject = gameObject;
        this.type = type;
    }
}

// public class EnemyPooler : MonoBehaviour
// {
//     public static EnemyPooler SharedInstance;
//     public List<EnemyPoolItem> enemiesToPool; // types of different object to pool 
//     public List<ExistingPoolEnemy> pooledEnemies; // a list of all objects in the pool, of all types
//     void Awake()
//     {
//         SharedInstance = this;
//         pooledEnemies = new List<ExistingPoolEnemy>();

//         foreach (EnemyPoolItem enemy in enemiesToPool)
//         {
//             for (int i = 0; i < enemy.amount; i++)
//             {
//                 // this 'instance' a local variable, but Unity will not remove it since it exists in the scene
//                 GameObject instance = (GameObject) Instantiate(enemy.prefab);
//                 instance.GetComponent<StateController>().SetWaypointList(enemy.waypoints);
//                 instance.GetComponent<StateController>().SetLastSeen(enemy.investigationPoint);
//                 instance.SetActive(false);
//                 instance.transform.parent = this.transform;

//                 ExistingPoolEnemy e = new ExistingPoolEnemy(instance, enemy.type);
//                 pooledEnemies.Add(e);
//             }
//         }
//     }

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     // this method can be called by other scripts to get pooled object by its type defined as enum earlier, or simly as tag as you like
//     // there's no "return" object to pool method. Simply set it as unavailable
//     public GameObject GetPooledEnemies(EnemyType type)
//     {
//         // return inactive pooled object if it matches the type 
//         for (int i = 0; i < pooledEnemies.Count; i++)
//         {
//             if (!pooledEnemies[i].gameObject.activeInHierarchy && pooledEnemies[i].type == type)
//             {
//                 return pooledEnemies[i].gameObject;
//             }
//         }
//         // this will be called when no more active object is present, item to expand pool if required 
//         foreach (EnemyPoolItem enemy in enemiesToPool)
//         {
//             if (enemy.type == type)
//             {
//                 if (enemy.expandPool)
//                 {
//                     GameObject instance = (GameObject) Instantiate (enemy.prefab);
//                     instance.SetActive(false);
//                     instance.transform.parent = this.transform;
//                     pooledEnemies.Add(new ExistingPoolEnemy(instance, enemy.type));
//                     return instance;
//                 }
//             }
//         }

//         // we will return null IF and only IF the type doesn't match with what is defined in the enemiesToPool. 
//         return null;
//     }

// }
