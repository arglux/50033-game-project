using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour {
    private Grid<HeatMapGridObject> grid;
    private Camera mainCamera;
    private Vector3 position;

    private void Start() {
        grid = new Grid<HeatMapGridObject>(5, 3, new Vector3(32, 0), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y) /*, 32, false */);
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }
    
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Setting.");
            position = GetMouseWorldPositionWithZ(Input.mousePosition, mainCamera);
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            if (heatMapGridObject != null) heatMapGridObject.AddValue(5);
        }
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("Getting.");
            position = GetMouseWorldPositionWithZ(Input.mousePosition, mainCamera);
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            Debug.Log(heatMapGridObject.GetValueNormalized());
        }
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}

public class HeatMapGridObject {
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue) {
        value += addValue;
        Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized() {
        return (float) value / MAX;
    }

    public override string ToString() {
        return value.ToString();
    }
}