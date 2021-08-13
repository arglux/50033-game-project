using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    public GameObject playerSetupMenuPrefab;
    private GameObject rootMenu;
    public PlayerInput input;

    public void Awake()
    {
        rootMenu = GameObject.Find("MainLayout");
        if(rootMenu != null)
        {
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSelectController>().setPlayerIndex(input.playerIndex);
            // Debug.Log(input.playerIndex + " Player assigned Menu(SpawnPlayerSetupMenu)");
        }
    }
}