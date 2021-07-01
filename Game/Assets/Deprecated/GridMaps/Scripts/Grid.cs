using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<GridObject> {

    // https://docs.microsoft.com/en-us/dotnet/api/system.eventargs?view=net-5.0
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private int cellSize;
    private bool showDebug;
    private GridObject[,] gridArray;
    private TextMesh[,] gridText;
    private Vector3 origin;

    private Vector3 offset;
    public const int sortingOrderDefault = 5000;

    public Grid(int width, int height, Vector3 origin, Func<Grid<GridObject>, int, int, GridObject> initGridObject, int cellSize = 32, bool showDebug = true) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.showDebug = showDebug;

        gridArray = new GridObject[width, height];
        gridText = new TextMesh[width, height];
        offset = new Vector3(cellSize, cellSize) * 0.5f;

        // init grid object
        for (int i=0; i<gridArray.GetLength(0); i++) {
            for (int j=0; j<gridArray.GetLength(1); j++) {
                gridArray[i, j] = initGridObject(this, i, j);
            }
        }

        if (showDebug) { // show the grid map
            for (int i=0; i<gridArray.GetLength(0); i++) {
                for (int j=0; j<gridArray.GetLength(1); j++) {
                    gridText[i, j] = CreateGridText(gridArray[i, j]?.ToString(), null, GetWorldPosition(i, j) + offset);
                    DrawBorders(i, j, cellSize);
                }
            }

            // subscribe to event with this lambda exp event handler
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-subscribe-to-and-unsubscribe-from-events#to-subscribe-to-events-programmatically
            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                gridText[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public GridObject GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetPosition(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public GridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < this.width && y < this.height) {
            return gridArray[x, y];
        } else {
            return default(GridObject);
        }
    }

    public void SetGridObject(Vector3 worldPosition, GridObject value) {
        int x, y;
        GetPosition(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public void GetPosition(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
    }

    public void SetGridObject(int x, int y, GridObject value) {
        if (x >= 0 && y >= 0 && x < this.width && y < this.height) {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {x = x, y = y});
        }
    }

    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {x = x, y = y});
    }

    private void DrawBorders(int x, int y, int cellSize) {
        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(x, y + 1), GetWorldPosition(x + 1, y + 1), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x + 1, y + 1), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + origin;
    }

    private static TextMesh CreateGridText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 64, Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
        if (color == null) color = Color.white;
        return CreateText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    
    private static TextMesh CreateText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("GridText", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public int GetHeight() {
        return height;
    }

    public int GetWidth() {
        return width;
    }

    public int GetCellSize() {
        return cellSize;
    }
}
