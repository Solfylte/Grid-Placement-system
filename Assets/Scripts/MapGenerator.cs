using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Map map;            // мапа (додаю з редактора)
    public Terrain terrain;    // поверхня (додаю з редактора)

    public SceneryManager sceneryManager;

    [SerializeField, Range(0, 1)]
    private float thickness;           // частота розміщення об'єктів

    // крок заповнення (мінімальна кількість клітинок, через які можливо буде розміщенний наступний об'єкт. Логічно виставити по максимальному наявному об'єкту)
    [SerializeField, Range(1, 6)]
    private ushort fillStep;

    public ushort fogOffset;
    public Transform fogPlane;

    // Start is called before the first frame update
    void Start()
    {
        BoardSceneryGenerate();
        TerrainGenerate();
        FogPlaneGenerate();
    }

    //Заповнення країв мапи об'єктами
    private void BoardSceneryGenerate()
    {
        if (sceneryManager.sceneryList.Count==0)
        {
            Debug.LogError("Not set sceneryes! (set in editor)");
            return;
        }

        for (int x = 1; x < map.XLength; x += fillStep)
        {
            for (int z = 1; z < map.ZLength; z += fillStep)
            {
                //пропустити клітинку, якщо вона поза визначеними межами "края" мапи
                if (z  > map.LenghtOfBoard && map.ZLength - z > map.LenghtOfBoard
                    && x > map.LenghtOfBoard && map.XLength - x > map.LenghtOfBoard)
                    continue;

                 if (UnityEngine.Random.Range(0, 1.0f) > thickness)      //перевірка на шанс встановлення декорації, відповідно до встановленної в редакторі густоти
                    continue;

                int index = UnityEngine.Random.Range(0, sceneryManager.sceneryList.Count);   //індекс декорації в листі

                if (!map.IsСellsEmpty(x, z, sceneryManager.sceneryList[index].Size))          //пропустити, якщо клітинка зайнята
                    continue;

                GameObject.Instantiate(sceneryManager.sceneryList[index].gameObject, sceneryManager.getPosition(x,z,map.CellSize,index), Quaternion.identity);

                map.markCellsAsBusy(x,z, sceneryManager.sceneryList[index].Size);
            }
        }
    }

    //  генерація поверхні
    private void TerrainGenerate()
    {
        if (terrain == null)
        {
            Debug.LogError("not set terrain! (set in editor)");
            return;
        }

        terrain.terrainData.size = new Vector3(map.CellSize * map.XLength, 1, map.CellSize * map.ZLength);  //розмір поверхні по розміру мапи
        terrain.transform.position = new Vector3(map.xStartPoint, 0, map.zStartPoint);          //початкова точка в початковій точці мапи
    }

    //
    private void FogPlaneGenerate()
    {
        setFogPanelTransform(Transform.Instantiate(fogPlane, new Vector3((map.CellSize*map.XLength)/2,-2f, (map.CellSize * (map.ZLength - fogOffset))),  Quaternion.identity), false, 3.2f);
        setFogPanelTransform(Transform.Instantiate(fogPlane, new Vector3((map.CellSize * (map.XLength - fogOffset)), -2f, (map.CellSize * map.ZLength)/2), Quaternion.identity), true, 3.2f);
        setFogPanelTransform(Transform.Instantiate(fogPlane, new Vector3(map.xStartPoint+fogOffset * map.CellSize, -2f, (map.CellSize * map.ZLength)/2), Quaternion.identity), true, 3.2f);
        //setFogPanelTransform(Transform.Instantiate(fogPlane, new Vector3((map.CellSize * map.XLength) / 2, -2f, map.zStartPoint+ fogOffset* map.CellSize), Quaternion.identity), true);

        setFogPanelTransform(Transform.Instantiate(fogPlane, new Vector3((map.CellSize * map.XLength) / 2, -2f, (map.CellSize * (map.ZLength - fogOffset-1))), Quaternion.identity), false, 5f);
        setFogPanelTransform(Transform.Instantiate(fogPlane, new Vector3((map.CellSize * (map.XLength - fogOffset-1)), -2f, (map.CellSize * map.ZLength) / 2), Quaternion.identity), true, 5f);
        setFogPanelTransform(Transform.Instantiate(fogPlane, new Vector3(map.xStartPoint + (fogOffset+1) * map.CellSize, -2f, (map.CellSize * map.ZLength) / 2), Quaternion.identity), true, 5f);
    }

    private void setFogPanelTransform(Transform fogPanel, bool isHorizontal, float scaleZ)
    {
        fogPanel.localScale = new Vector3(map.XLength / 8, 0, scaleZ);
        fogPanel.Rotate(90, isHorizontal?90:0, 0);
    }
}