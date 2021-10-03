using System.Collections.Generic;
using UnityEngine;

public class SceneryManager : MonoBehaviour
{
    public Camera cam;
    public List<Scenery> sceneryList;

    //Визначення координат для установки даного об'єкта
    public Vector3 getPosition(int x, int z, float cellSize, int index)
    {
         bool isOffset = sceneryList[index].Size % 2 == 0;
        float offsetToCenter = !isOffset ? cellSize / 2f : 0;
        return new Vector3(x * cellSize + offsetToCenter, 0, z * cellSize + offsetToCenter);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GetInformationAboutScenery();
    }

    private void GetInformationAboutScenery()
    {
        if (cam == null)
            return;

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;

            if (objectHit.tag != "scenery")
                return;

            Scenery sceneryHit = objectHit.GetComponent<Scenery>();
            if (true)
            {
                Debug.Log(GetTextInfo(sceneryHit));
            }
        }
    }

    public static string GetTextInfo(Scenery scenery) => $"Name: {scenery.DisplayedName}; Size: {scenery.Size }x{scenery.Size}; ID={scenery.ID}";

}