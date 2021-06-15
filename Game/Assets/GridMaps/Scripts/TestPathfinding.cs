using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MonoBehaviour {

    private Pathfinding pathfinding;
    private Camera mainCamera;
    private Vector3 position;

    void Start() {
        pathfinding = new Pathfinding(10, 10);
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            position = GetMouseWorldPositionWithZ(Input.mousePosition, mainCamera);
            pathfinding.GetGrid().GetPosition(position, out int x, out int y);
            
            Vector3 offset = Vector3.one * 16f;
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null) {
                for (int i=0; i < path.Count - 1; i++) {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 32f + offset, new Vector3(path[i+1].x, path[i+1].y) * 32f + offset, Color.green, 2);
                    Debug.Log(path[i].x + "," + path[i].y);
                }
                
            }
            
        }
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
