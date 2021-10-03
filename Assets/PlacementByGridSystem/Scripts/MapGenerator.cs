using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Terrain terrain;

    public SceneryManager sceneryManager;

    [SerializeField, Range(0, 1)] private float thickness;
    [SerializeField, Range(1, 6)] private ushort fillStep;
    [SerializeField, Range(1, 6)] private ushort lenghtOfBoard;

    void Start()
    {
        BoardSceneryGenerate();
        TerrainGenerate();
    }

    private void BoardSceneryGenerate()
    {
        if (sceneryManager.sceneryList.Count==0)
            return;

        for (int x = 1; x < grid.Width; x += fillStep)
            for (int y = 1; y < grid.Height; y += fillStep)
            {
                if (y  > lenghtOfBoard && grid.Height - y > lenghtOfBoard
                    && x > lenghtOfBoard && grid.Width - x > lenghtOfBoard)
                    continue;

                 if (UnityEngine.Random.Range(0, 1.0f) > thickness)
                    continue;

                int index = UnityEngine.Random.Range(0, sceneryManager.sceneryList.Count);

                if (!grid.IsСellsEmpty(x, y, sceneryManager.sceneryList[index].Size))
                    continue;

                GameObject.Instantiate(sceneryManager.sceneryList[index].gameObject, sceneryManager.getPosition(x, y, grid.CellSize, index), Quaternion.identity);

                grid.MarkCellsAsFilled(x, y, sceneryManager.sceneryList[index].Size);
            }
    }

    private void TerrainGenerate()
    {
        if (!terrain)
            return;

        terrain.terrainData.size = new Vector3(grid.CellSize * grid.Width, 1, grid.CellSize * grid.Height);
        terrain.transform.position = new Vector3(grid.Origin.x, 0, grid.Origin.y);
    }
}