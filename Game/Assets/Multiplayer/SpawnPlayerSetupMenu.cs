using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    public GameObject playerSetupMenuPrefab;
    private GameObject row1;
    private GameObject row2;
    public PlayerInput input;
    
    public void Awake()
    {
        row1 = GameObject.Find("Row1");
        row2 = GameObject.Find("Row2");
        if (row1 != null && row2 != null) {
            if (input.playerIndex<=1) {
                var menu = Instantiate(playerSetupMenuPrefab, row1.transform);
                input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSelectController>().setPlayerIndex(input.playerIndex);
            } else if (input.playerIndex==2) {
                RectTransform rt1 = row1.GetComponent<RectTransform>();
                rt1.offsetMin = new Vector2(rt1.offsetMin.x, 540);
                rt1.offsetMax = new Vector2(rt1.offsetMax.x, -140);
                var menu = Instantiate(playerSetupMenuPrefab, row2.transform);
                input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSelectController>().setPlayerIndex(input.playerIndex);
            } else if (input.playerIndex==3) {
                var menu = Instantiate(playerSetupMenuPrefab, row2.transform);
                input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSelectController>().setPlayerIndex(input.playerIndex);
            } else {
                Debug.Log("Player limit exceeded");
            }
        } else {
            Debug.Log("Failed to initialize");
        }
    }
}