using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    public SceneryManager sceneryManager;          //менеджер сценері (встановлюєтсья в редакторі)
    public GameObject placementObject;             //Об'єкт, який додається на мапу

    public GameObject fillCellSprite;               // Префаб спрайта підсвітки клітинки під об'єктом який додається на мапу(додаю з редактора)
    private List<SpriteRenderer> fillCellSpriteRenders = new List<SpriteRenderer>();    //Список спрайтів підсвітки клітинок (формується по кількості клітинок які займає )
    public Map map;

    public Camera cam;
    public AudioSource setScenerySound;

    public TouchEventSystem touchEventSystem;

    private ushort scenerySize;

    void Start()
    {
        touchEventSystem.doubleTouchMessage += OnEventSetScenery;
    }

    // Update is called once per frame
    void Update()
    {
        if (placementObject == null)
            return;

         SetGridPosition();

        //CheckToSet();
    }

    private Vector3 SetRaycastPosition()
    {
        if (cam == null)
            return Vector3.zero;

         RaycastHit hit;
         Ray ray = cam.ScreenPointToRay(Input.mousePosition);

         if (Physics.Raycast(ray, out hit))
         {
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.tag != "map")
                return Vector3.zero;

            return hit.point;
         }

        return Vector3.zero;
    }

    private void SetGridPosition()
    {
          Vector3 realPosition = SetRaycastPosition();
          if (realPosition==Vector3.zero)
              return;

        placementObject.transform.position =  sceneryManager.getPosition(cellX(realPosition), cellZ(realPosition), map.CellSize, scenerySize);
        SetCellFillSpritePosition(cellX(realPosition), cellZ(realPosition), scenerySize);
    }

    private int cellX(Vector3 realPosition)=> (int)((realPosition.x - map.xStartPoint) / map.CellSize);
    private int cellZ(Vector3 realPosition) => (int)((realPosition.z - map.zStartPoint) / map.CellSize);

    private void SetCellFillSpritePosition(int cellX, int cellZ, ushort scenerySize)
     {
         if (fillCellSpriteRenders.Count == 0)
             return;

        Cell[] cells = map.getCells(cellX, cellZ, scenerySize);

        if (cells.Length!= fillCellSpriteRenders.Count)
        {
            Debug.LogError($"cells.Length!= fillCellSpriteRenders.Count. Cell={cells.Length} fillCellSpriteRenders={fillCellSpriteRenders.Count}");
            return;
        }

        for (int i = 0; i < fillCellSpriteRenders.Count; i++)
        {
            if (cells[i]==null)
            {
                //Debug.Log($"cells[{i}]==null");
                continue;
            }

            fillCellSpriteRenders[i].gameObject.transform.position = new Vector3(cells[i].x *cells[i].cellSize+ cells[i].cellSize/2, 0.1f, cells[i].z * cells[i].cellSize + cells[i].cellSize / 2);

            if (cells[i].isFill)
                fillCellSpriteRenders[i].color = Color.red;
            else
                fillCellSpriteRenders[i].color = Color.green;
        }
     }

    private void AddCellFillSprite()
    {
        if (fillCellSprite == null)
            return;

        for (int i = 0; i < scenerySize * scenerySize; i++)
            fillCellSpriteRenders.Add(GameObject.Instantiate(fillCellSprite).GetComponent<SpriteRenderer>());
    }

    private void ClearCellFillSprite()
    {
        int count = fillCellSpriteRenders.Count;

        for (int i = 0; i < count; i++)
            GameObject.Destroy(fillCellSpriteRenders[i].gameObject);

        fillCellSpriteRenders.Clear();
    }    

    public void OnEventButtonAddScenery(AddSceneryButton sceneryButton)
    {
        if (placementObject != null)
        {
            Destroy(placementObject);
            ClearCellFillSprite();
        }

        scenerySize = sceneryButton.scenery.Size;
        placementObject = GameObject.Instantiate(sceneryButton.scenery.gameObject, map.transform.position, Quaternion.identity);
        placementObject.GetComponent<MeshCollider>().enabled = false;
        AddCellFillSprite();
    }

    public void OnEventButtonCancel()       //Відключив...
    {
        Destroy(placementObject);
        ClearCellFillSprite();
    }

    public void OnEventSetScenery()
    {
        if (placementObject == null)
            return;

        if (!map.IsСellsEmpty(cellX(placementObject.transform.position), cellZ(placementObject.transform.position), scenerySize))
            return;

        if (placementObject != null && setScenerySound != null)
            setScenerySound.Play();

        map.markCellsAsBusy(cellX(placementObject.transform.position), cellZ(placementObject.transform.position), scenerySize);

        placementObject.GetComponent<MeshCollider>().enabled = true;
        placementObject = null;

        ClearCellFillSprite();
    }
}