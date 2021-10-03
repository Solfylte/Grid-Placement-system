using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public float cellSize;
    public bool isFill;

    public int x;
    public int z;

    public Cell(int x, int z, float cellSize)
    {
        this.x = x;
        this.z = z;
        this.cellSize = cellSize;
    }
}
