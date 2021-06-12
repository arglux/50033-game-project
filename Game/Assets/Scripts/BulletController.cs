using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public GameObject hitEffect;

    
    void OnCollisionEnter2D(Collision2D other) {
        // create hit effect
        Debug.Log(other);
        Destroy(gameObject);
    }

}
