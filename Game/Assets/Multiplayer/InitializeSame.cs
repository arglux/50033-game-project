using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSame : MonoBehaviour
{
    [SerializeField]
    private Transform[] PlayerSpawns;
    private List<Camera> cameras;

    [SerializeField]
    private GameObject playerPrefab;

    private void Awake()
    {
        cameras = new List<Camera>();
        // Debug.Log("IntializeSame Script");
    }

    public void UpdatePlayers(PlayerConfiguration pc)
    {
        if (!playerPrefab) {
            // Debug.Log("Insert a player prefab");
            return;
        }
        int i = pc.playerIndex;
        // Debug.Log("Init SAME playerIndex: "+ i);
        var player = Instantiate(playerPrefab, PlayerSpawns[i].position, PlayerSpawns[i].rotation, gameObject.transform);
        PlayerHandler ph = player.transform.GetComponent<PlayerHandler>();
        ph.InitializeHandler(pc);
        cameras.Add(ph.playerCam);
        switch(cameras.Count) {
            // TODO 4 cameras
            case 3:
                cameras[0].rect = new Rect(0, .5f, 1.0f, .5f);
                cameras[1].rect= new Rect(0, 0, .5f, .5f);
                cameras[2].rect= new Rect(.5f, 0, .5f, .5f);
                break;
            case 2:
                cameras[0].rect = new Rect(0, 0, .5f, 1.0f);
                cameras[1].rect= new Rect(.5f, 0, .5f, 1.0f);
                break;
            case 1:
                cameras[0].rect= new Rect(0f, 0f, 1.0f, 1.0f);
                break;
            default:
                // Debug.Log("Camera initialization failed");
                break;
        }
        
    }
}