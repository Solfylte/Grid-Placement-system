using System;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public float CellSize => cellSize;
    public int Width => width;
    public int Height => height;
    public Vector2 Origin => origin;
    public Cell[,] Cells => cells;

    private Cell[,] cells = new Cell[5,5];

    [SerializeField] private Vector2 origin;
    [SerializeField, Range(0.5f, 5.0f)] private float cellSize;
    [SerializeField, Range(1, 96)] private int width = 16;
    [SerializeField, Range(1, 96)] private int height = 16;

    void Awake()
    {
        CreateGrid();

        int rows = cells.GetUpperBound(0) + 1;
        int columns = cells.Length / rows;
        Debug.Log($"GridLength={cells.Length} rows={rows} col={columns}");

        transform.position = new Vector3(origin.x+(rows * cellSize)/2, 0, origin.y + (columns * cellSize) / 2);
        transform.localScale = new Vector3(rows * cellSize, 0.5f, columns * cellSize);
    }

    private void CreateGrid()
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell(x, y, cellSize);
                cells[x, y] = cell;
            }
    }

    public void MarkCellsAsFilled(int x, int y, ushort size)
    {
        Cell[] cells = GetCells(x, y, size);
        foreach (Cell cell in cells)
            cell.isFill = true;
    }

    public Cell[] GetCells(int x, int y, ushort size)
    {
        Cell[] result = new Cell[size * size];

        bool offset = size % 2 == 0 ? true : false;

        int Xstart = x - (size - 1) / 2 - (offset ? 1 : 0);
        int Ystart = y - (size - 1) / 2 - (offset ? 1 : 0);

        int index = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (j + Ystart >= height || i + Xstart >= width || j + Ystart<0 || i + Xstart < 0)
                    continue;

                result[index] = cells[i+ Xstart, j+ Ystart];
                index++;
            }
        }
        
        return result;
    }

    public bool IsСellsEmpty(int x, int y, ushort size)
    {
        foreach (Cell cell in GetCells(x, y, size))
            if (cell!=null)
                if (cell.isFill)
                    return false;

        return true;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (cells.Length == 0)
            return;

        int rows = cells.GetUpperBound(0) + 1;
        int columns = cells.Length / rows;

        Gizmos.color = Color.yellow;

         for (int i = 0; i < width+1; i++)
            Gizmos.DrawLine(new Vector3(origin.x + cellSize * i, 0, origin.y),
                                new Vector3(origin.x + cellSize * i, 0, origin.y + cellSize * height));

        for (int i = 0; i < height+1; i++)
            Gizmos.DrawLine(new Vector3(origin.x , 0, origin.y + cellSize * i),
                                new Vector3(origin.x + cellSize * width, 0, origin.y + cellSize * i));
    }
#endif
}
