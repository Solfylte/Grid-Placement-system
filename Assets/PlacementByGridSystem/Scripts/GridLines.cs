using System.Collections.Generic;
using UnityEngine;

public class GridLines : MonoBehaviour
{
    public Grid grid;
    private LineRenderer lineRender;

    [SerializeField, Range(0.01f, 0.2f)]
    private float offset;

    void Start()
    {
        lineRender = gameObject.GetComponent<LineRenderer>();
        gridLineRenderGenerate();
    }

    private void gridLineRenderGenerate()
    {
        if (grid == null)
        {
            Debug.LogError("map not set!");
            return;
        }

        List<Vector3> lineRenderPositons = new List<Vector3>();
        
        lineRenderPositons.Add(new Vector3(grid.Origin.x, offset, grid.Origin.y));

        for (int x = 0; x < grid.Origin.x + 1; x++)
        {
            float zPos = x % 2 == 0 ? grid.Origin.y + grid.CellSize * grid.Height : 0;
            float xPos = grid.Origin.x + grid.CellSize * x;

            lineRenderPositons.Add(new Vector3(xPos, offset, zPos));

            if (x < grid.Origin.x)
                lineRenderPositons.Add(new Vector3(xPos+ grid.CellSize, offset, zPos));
        }

        lineRenderPositons.Add(new Vector3((grid.Width%2==0?(grid.Origin.x+ grid.CellSize * grid.Origin.x) : grid.Origin.x), offset, grid.Origin.y));

        for (int z = 0; z < grid.Height + 1; z++)
        {
            float xPos = z % 2 != 0 ? grid.Origin.x + grid.CellSize * grid.Width : 0;
            float zPos = grid.Origin.y + grid.CellSize * z;

            lineRenderPositons.Add(new Vector3(xPos, offset, zPos));

            if (z < grid.Height)
                lineRenderPositons.Add(new Vector3(xPos , offset, zPos + grid.CellSize));
        }

        lineRender.positionCount = lineRenderPositons.Count;
        lineRender.SetPositions(lineRenderPositons.ToArray());
    }
}