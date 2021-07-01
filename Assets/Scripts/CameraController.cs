using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private float z;

    void Start()
    {
        this.z = this.gameObject.transform.position.z;
    }

    void LateUpdate()
    {
        Vector3 position = this.player.transform.position;
        this.gameObject.transform.position = new Vector3(position.x, position.y, this.z);
    }
}
