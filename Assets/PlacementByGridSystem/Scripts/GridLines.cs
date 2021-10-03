using System.Collections.Generic;
using UnityEngine;

public class GridLines : MonoBehaviour
{
    public Map map;                     // поточна мапа (додаю з редактора)
    private LineRenderer lineRender;

    [SerializeField, Range(0.01f, 0.2f)]
    private float offset;               //відступ сітки над мапою

    void Start()
    {
        lineRender = gameObject.GetComponent<LineRenderer>();           //Скрипт знаходиться на об'єкті що містить LineRenderer
        gridLineRenderGenerate();
    }

    private void gridLineRenderGenerate()
    {
        if (map == null)
        {
            Debug.LogError("map not set!");
            return;
        }

        List<Vector3> lineRenderPositons = new List<Vector3>();     //список точок, по яким послідовно побудувати лінію
        
        lineRenderPositons.Add(new Vector3(map.xStartPoint, offset, map.zStartPoint)); //початкова точка

        //побудова ліній сітки
        for (int x = 0; x < map.XLength + 1; x++)
        {
            float zPos = x % 2 == 0 ? map.zStartPoint + map.CellSize * map.ZLength : 0;
            float xPos = map.xStartPoint + map.CellSize * x;

            lineRenderPositons.Add(new Vector3(xPos, offset, zPos));

            if (x < map.XLength)
                lineRenderPositons.Add(new Vector3(xPos+ map.CellSize, offset, zPos));
        }

        lineRenderPositons.Add(new Vector3((map.XLength%2==0?(map.xStartPoint+ map.CellSize * map.XLength) : map.xStartPoint), offset, map.zStartPoint));        //горизонталь початкова точка

        //побудова ліній сітки
        for (int z = 0; z < map.ZLength + 1; z++)
        {
            float xPos = z % 2 != 0 ? map.xStartPoint + map.CellSize * map.XLength : 0;
            float zPos = map.zStartPoint + map.CellSize * z;

            lineRenderPositons.Add(new Vector3(xPos, offset, zPos));

            if (z < map.ZLength)
                lineRenderPositons.Add(new Vector3(xPos , offset, zPos + map.CellSize));
        }

        lineRender.positionCount = lineRenderPositons.Count;
        lineRender.SetPositions(lineRenderPositons.ToArray());
    }
}