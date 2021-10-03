using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    [SerializeField] private SceneryManager sceneryManager;
    [SerializeField] private GameObject placementObject;

    [SerializeField] private GameObject cellHighlightSpritePrefab;
    private List<SpriteRenderer> fillCellSpriteRenders = new List<SpriteRenderer>();
    [SerializeField] private Grid grid;

    [SerializeField] private Camera cam;
    [SerializeField] private AudioSource setScenerySound;

    [SerializeField] private TouchEventSystem touchEventSystem;

    private ushort scenerySize;

    void Start()
    {
        touchEventSystem.doubleTouchMessage += OnEventSetScenery;
    }

    void Update()
    {
        if (placementObject == null)
            return;

         SetGridPosition();
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

        placementObject.transform.position =  sceneryManager.getPosition(cellX(realPosition), cellZ(realPosition), grid.CellSize, scenerySize);
        SetCellFillSpritePosition(cellX(realPosition), cellZ(realPosition), scenerySize);
    }

    private int cellX(Vector3 realPosition)=> (int)((realPosition.x - grid.Origin.x) / grid.CellSize);
    private int cellZ(Vector3 realPosition) => (int)((realPosition.z - grid.Origin.y) / grid.CellSize);

    private void SetCellFillSpritePosition(int cellX, int cellZ, ushort scenerySize)
     {
         if (fillCellSpriteRenders.Count == 0)
             return;

        Cell[] cells = grid.GetCells(cellX, cellZ, scenerySize);

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
        if (cellHighlightSpritePrefab == null)
            return;

        for (int i = 0; i < scenerySize * scenerySize; i++)
            fillCellSpriteRenders.Add(GameObject.Instantiate(cellHighlightSpritePrefab).GetComponent<SpriteRenderer>());
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
        placementObject = GameObject.Instantiate(sceneryButton.scenery.gameObject, grid.transform.position, Quaternion.identity);
        placementObject.GetComponent<MeshCollider>().enabled = false;
        AddCellFillSprite();
    }

    public void OnEventSetScenery()
    {
        if (placementObject == null)
            return;

        if (!grid.IsСellsEmpty(cellX(placementObject.transform.position), cellZ(placementObject.transform.position), scenerySize))
            return;

        if (placementObject != null && setScenerySound != null)
            setScenerySound.Play();

        grid.MarkCellsAsFilled(cellX(placementObject.transform.position), cellZ(placementObject.transform.position), scenerySize);

        placementObject.GetComponent<MeshCollider>().enabled = true;
        placementObject = null;

        ClearCellFillSprite();
    }
}